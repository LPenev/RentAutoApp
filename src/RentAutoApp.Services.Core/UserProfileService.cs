using Microsoft.AspNetCore.Identity;
using RentAutoApp.Data.Models;
using RentAutoApp.Services.Common;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Services.Core.Dtos;

namespace RentAutoApp.Services.Core;

public class UserProfileService : IUserProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserProfileService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result<UserProfileDto>> GetAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result<UserProfileDto>.Fail("Потребителят не е намерен.");

        var dto = new UserProfileDto(
            Email: user.Email ?? string.Empty,
            FirstName: user.FirstName ?? string.Empty,
            LastName: user.LastName ?? string.Empty,
            PhoneNumber: user.PhoneNumber
        );

        return Result<UserProfileDto>.Success(dto);
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileCommand cmd)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result.Fail("Потребителят не е намерен.");

        // Keep non-nullable constraints, trim values, fallback to current if null
        user.FirstName = (cmd.FirstName ?? user.FirstName ?? string.Empty).Trim();
        user.LastName = (cmd.LastName ?? user.LastName ?? string.Empty).Trim();
        user.PhoneNumber = string.IsNullOrWhiteSpace(cmd.PhoneNumber)
            ? null
            : cmd.PhoneNumber!.Trim();

        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded) return FromIdentity(res);

        await _signInManager.RefreshSignInAsync(user);
        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result.Fail("Потребителят не е намерен.");

        var res = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!res.Succeeded) return FromIdentity(res);

        await _signInManager.RefreshSignInAsync(user);
        return Result.Success();
    }

    public async Task<Result> ChangeEmailAsync(string userId, string newEmail, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result.Fail("Потребителят не е намерен.");

        if (!await _userManager.CheckPasswordAsync(user, password))
            return Result.Fail("Невалидна парола.");

        if (string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
            return Result.Fail("Новият имейл съвпада с текущия.");

        // Ensure unique email
        var existing = await _userManager.FindByEmailAsync(newEmail);
        if (existing is not null && existing.Id != user.Id)
            return Result.Fail("Имейлът вече се използва.");

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        var res = await _userManager.ChangeEmailAsync(user, newEmail, token);
        if (!res.Succeeded) return FromIdentity(res);

        // Sync username if it was same as email
        if (string.Equals(user.UserName, user.Email, StringComparison.OrdinalIgnoreCase))
        {
            user.UserName = newEmail;
            var res2 = await _userManager.UpdateAsync(user);
            if (!res2.Succeeded) return FromIdentity(res2);
        }

        await _signInManager.RefreshSignInAsync(user);
        return Result.Success();
    }

    private static Result FromIdentity(IdentityResult idRes)
        => idRes.Succeeded
           ? Result.Success()
           : Result.Fail(string.Join("; ", idRes.Errors.Select(e => e.Description)));
}

