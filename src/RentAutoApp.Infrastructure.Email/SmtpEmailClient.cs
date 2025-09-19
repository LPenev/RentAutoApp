using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Infrastructure.Email.Contracts;
using Microsoft.Extensions.Logging;

namespace RentAutoApp.Infrastructure.Email;

public sealed class SmtpEmailClient : IEmailClient
{
    private readonly SmtpOptions _s;
    private readonly ILogger<SmtpEmailClient> _log;
    public SmtpEmailClient(IOptions<EmailSettings> opt, ILogger<SmtpEmailClient> log)
    {
        _s = opt.Value.Smtp;
        _log = log;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
    {
        using var client = new SmtpClient(_s.Host, _s.Port)
        {
            EnableSsl = _s.EnableSsl,
            Timeout = 10000, // 10s timeout
            Credentials = string.IsNullOrWhiteSpace(_s.User)
                                               ? CredentialCache.DefaultNetworkCredentials
                                                                  : new NetworkCredential(_s.User, _s.Password)
        };

        using var mail = new MailMessage(
                string.IsNullOrWhiteSpace(message.From) ? _s.From : message.From,
                message.To,
                message.Subject,
                message.HtmlBody
            )
        { IsBodyHtml = true };

        // Reply-To
        if (!string.IsNullOrWhiteSpace(message.ReplyTo))
            mail.ReplyToList.Add(new MailAddress(message.ReplyTo));

        // CC
        if (message.Cc is { Count: > 0 })
            foreach (var cc in message.Cc)
                if (!string.IsNullOrWhiteSpace(cc)) mail.CC.Add(cc);

        // BCC
        foreach (var bcc in message.Bcc)
            if (!string.IsNullOrWhiteSpace(bcc)) mail.Bcc.Add(bcc);

        // Attachments
        if (message.Attachments is { Count: > 0 })
        {
            foreach (var att in message.Attachments)
            {
                var stream = new MemoryStream(att.Content, writable: false);
                var attachment = att.ContentType is { Length: > 0 }
                                    ? new Attachment(stream, att.FileName, att.ContentType)
                                       : new Attachment(stream, att.FileName);
                mail.Attachments.Add(attachment);
            }
        }

        _log.LogInformation("SMTP sending to {To} via {Host}:{Port} (SSL={Ssl})", message.To, _s.Host, _s.Port, _s.EnableSsl);

        var sendTask = client.SendMailAsync(mail);
        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(15), ct);
        var completed = await Task.WhenAny(sendTask, timeoutTask);
        if (completed != sendTask)
        {
            throw new TimeoutException($"SMTP send timed out to {_s.Host}:{_s.Port} (SSL={_s.EnableSsl}). Check port/TLS/credentials.");
        }
        await sendTask; // пропагира реалната Exception, ако има

        _log.LogInformation("SMTP sent to {To}", message.To);
    }
}