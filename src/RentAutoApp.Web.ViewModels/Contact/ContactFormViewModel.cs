using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.ViewModels.Contact;

public class ContactFormViewModel
{
    [Required(ErrorMessage = "Validation.Required")]
    [StringLength(80, ErrorMessage = "Validation.StringLengthMax")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Validation.Required")]
    [EmailAddress(ErrorMessage = "Validation.Email")]
    [StringLength(256, ErrorMessage = "Validation.StringLengthMax")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Validation.Phone")]
    [StringLength(32, ErrorMessage = "Validation.StringLengthMax")]
    public string? Phone { get; set; }

    [StringLength(120, ErrorMessage = "Validation.StringLengthMax")]
    public string? Subject { get; set; }

    [Required(ErrorMessage = "Validation.Required")]
    [StringLength(4000, MinimumLength = 5, ErrorMessage = "Validation.StringLengthRange")]
    public string Message { get; set; } = string.Empty;
}

