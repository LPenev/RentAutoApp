using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Services.Messaging.Contracts;

namespace RentAutoApp.Web.Infrastructure.Email;

public sealed class IdentityEmailSenderAdapter : IEmailSender
{
    private readonly IEmailService _svc;
    private readonly EmailSettings _settings;

    public IdentityEmailSenderAdapter(IEmailService svc, IOptions<EmailSettings> opt)
    {
        _svc = svc;
        _settings = opt.Value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    => _svc.SendAsync(new EmailMessage
    {
        To = email,
        Subject = subject,
        HtmlBody = htmlMessage,
        From = _settings.Smtp.From
    });
}


