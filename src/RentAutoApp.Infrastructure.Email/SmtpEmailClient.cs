using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Infrastructure.Email.Contracts;


namespace RentAutoApp.Infrastructure.Email;

    public sealed class SmtpEmailClient : IEmailClient
{
        private readonly SmtpOptions _s;
    public SmtpEmailClient(IOptions<EmailSettings> opt) => _s = opt.Value.Smtp;

        public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
    {
            using var client = new SmtpClient(_s.Host, _s.Port)
                                        {
                    EnableSsl = _s.EnableSsl,
                    Credentials = string.IsNullOrWhiteSpace(_s.User)
                                                   ? CredentialCache.DefaultNetworkCredentials
                                                                      : new NetworkCredential(_s.User, _s.Password)
                                                                                 };
    
                using var mail = new MailMessage(
                        string.IsNullOrWhiteSpace(message.From)? _s.From : message.From,
                        message.To,
                        message.Subject,
                        message.HtmlBody
                    )
        { IsBodyHtml = true };
    
                if (!string.IsNullOrWhiteSpace(message.ReplyTo))
                    mail.ReplyToList.Add(new MailAddress(message.ReplyTo));
    
                foreach (var cc in message.Cc)
                    if (!string.IsNullOrWhiteSpace(cc)) mail.CC.Add(cc);
    
                foreach (var bcc in message.Bcc)
                    if (!string.IsNullOrWhiteSpace(bcc)) mail.Bcc.Add(bcc);
    
                await client.SendMailAsync(mail);
        }
}