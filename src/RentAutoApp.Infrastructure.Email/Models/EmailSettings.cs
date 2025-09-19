using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Infrastructure.Email.Models;

public class EmailSettings
{
    [Required]
    public SmtpOptions Smtp { get; set; } = new();
}

public class SmtpOptions
{
    [Required, MinLength(1)]
    public string Host { get; set; } = default!;
    [Range(1, 65535)]
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    [EmailAddress]
    public string From { get; set; } = default!;
    public string? User { get; set; }
    public string? Password { get; set; }
}

