using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.ViewModels.Contact;

public class ContactFormViewModel
{
    [Required(ErrorMessage = "Име е задължително")]
    [StringLength(80, ErrorMessage = "Името е твърде дълго")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email е задължителен")]
    [EmailAddress(ErrorMessage = "Невалиден email")]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Невалиден телефон")]
    [StringLength(32)]
    public string? Phone { get; set; }

    [StringLength(120)]
    public string? Subject { get; set; }

    [Required(ErrorMessage = "Съобщението е задължително")]
    [StringLength(4000, MinimumLength = 5, ErrorMessage = "Съобщението е твърде кратко")]
    public string Message { get; set; } = string.Empty;
}

