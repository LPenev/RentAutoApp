using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace RentAutoApp.Web.Infrastructure.Email;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly SmtpOptions _s;
    public SmtpEmailSender(IOptions<EmailSettings> opt) => _s = opt.Value.Smtp;

    public async Task SendEmailAsync(string to, string subject, string html)
    {
        using var client = new SmtpClient(_s.Host, _s.Port)
        {
            EnableSsl = _s.EnableSsl,
            Credentials = new NetworkCredential(_s.User, _s.Password)
        };

        
        using var msg = new MailMessage(_s.From, to, subject, html) { IsBodyHtml = true };
        await client.SendMailAsync(msg);
    }
}
