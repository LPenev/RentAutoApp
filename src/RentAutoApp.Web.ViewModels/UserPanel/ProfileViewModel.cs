using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.ViewModels.UserPanel;
public class ProfileViewModel
{
    [Display(Name = "Имейл")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Собствено име"), StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Фамилия"), StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Телефон"), StringLength(30)]
    public string? PhoneNumber { get; set; }
}
