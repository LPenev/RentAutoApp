using RentAutoApp.Services.Common;
using RentAutoApp.Services.Core.Dtos;

namespace RentAutoApp.Services.Core.Contracts;

public interface IUserProfileService
{
    Task<Result<UserProfileDto>> GetAsync(string userId);

    Task<Result> UpdateProfileAsync(string userId, UpdateProfileCommand command);

    Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

    // Direct email change without separate confirmation (NoOpEmailSender)
    Task<Result> ChangeEmailAsync(string userId, string newEmail, string password);
}

