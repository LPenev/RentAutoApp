using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.ViewModels.UserPanel;

public class ChangeEmailViewModel
{
    [Required, EmailAddress, Display(Name = "Нов имейл")]
    public string NewEmail { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Display(Name = "Вашата парола")]
    public string Password { get; set; } = string.Empty;
}

