using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.Infrastructure.Email;

public class EmailTestOptions
{
    public bool Enabled { get; set; } = true;
    [EmailAddress] 
    public string? DefaultRecipient { get; set; }
    [Required] 
    public string Subject { get; set; } = "Test email";
    [Required] 
    public string BodyHtml { get; set; } = "<b>It works!</b>";
}
