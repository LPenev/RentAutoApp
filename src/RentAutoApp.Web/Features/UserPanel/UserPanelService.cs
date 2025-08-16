using RentAutoApp.Services.Common;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Services.Core.Dtos;
using RentAutoApp.Web.Infrastructure.Contracts;
using RentAutoApp.Web.ViewModels.UserPanel;

namespace RentAutoApp.Web.Features.UserPanel;
public class UserPanelService : IUserPanelService
{
    private readonly IUserProfileService _core;
    private readonly ICurrentUserService _current;

    public UserPanelService(IUserProfileService core, ICurrentUserService current)
    {
        _core = core;
        _current = current;
    }

    public async Task<Result<ProfileViewModel>> GetProfileAsync()
    {
        var me = await _core.GetAsync(_current.UserId!);
        if (!me.Succeeded) return Result<ProfileViewModel>.Fail(me.Error!);

        var vm = new ProfileViewModel
        {
            Email = me.Value!.Email,
            FirstName = me.Value!.FirstName,
            LastName = me.Value!.LastName,
            PhoneNumber = me.Value!.PhoneNumber
        };
        return Result<ProfileViewModel>.Success(vm);
    }

    public async Task<Result> UpdatePersonalAsync(ProfileViewModel vm)
    {
        var cmd = new UpdateProfileCommand(vm.FirstName, vm.LastName, vm.PhoneNumber);
        return await _core.UpdateProfileAsync(_current.UserId!, cmd);
    }

    public Task<Result> ChangeEmailAsync(ChangeEmailViewModel vm)
        => _core.ChangeEmailAsync(_current.UserId!, vm.NewEmail, vm.Password);

    public Task<Result> ChangePasswordAsync(ChangePasswordViewModel vm)
        => _core.ChangePasswordAsync(_current.UserId!, vm.CurrentPassword, vm.NewPassword);
}
