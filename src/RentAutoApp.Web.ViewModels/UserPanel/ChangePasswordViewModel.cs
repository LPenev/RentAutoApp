using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.ViewModels.UserPanel;

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password), Display(Name = "Текуща парола")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Display(Name = "Нова парола")]
    public string NewPassword { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Display(Name = "Потвърди новата парола")]
    [Compare(nameof(NewPassword), ErrorMessage = "Паролите не съвпадат.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

