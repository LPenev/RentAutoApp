using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.Infrastructure.Email;

public class EmailSettings
{
    public SmtpOptions Smtp { get; set; } = new();
}

public class SmtpOptions
{
    [Required]
    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    [EmailAddress]
    public string From { get; set; } = default!;
    public string? User { get; set; }
    public string? Password { get; set; }
}
