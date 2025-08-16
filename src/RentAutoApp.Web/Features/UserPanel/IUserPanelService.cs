using RentAutoApp.Services.Common;
using RentAutoApp.Web.ViewModels.UserPanel;

namespace RentAutoApp.Web.Features.UserPanel;

public interface IUserPanelService
{
    Task<Result<ProfileViewModel>> GetProfileAsync();
    Task<Result> UpdatePersonalAsync(ProfileViewModel vm);
    Task<Result> ChangeEmailAsync(ChangeEmailViewModel vm);
    Task<Result> ChangePasswordAsync(ChangePasswordViewModel vm);
}