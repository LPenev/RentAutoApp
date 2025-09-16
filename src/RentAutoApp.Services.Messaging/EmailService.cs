using Microsoft.Extensions.Logging;
using RentAutoApp.Infrastructure.Email.Contracts;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Services.Messaging.Contracts;

namespace RentAutoApp.Services.Messaging;

public sealed class EmailService : IEmailService
{
    private readonly IEmailClient _client;
    private readonly ILogger<EmailService> _log;

    public EmailService(IEmailClient client, ILogger<EmailService> log)
    => (_client, _log) = (client, log);

    public async Task SendAsync(EmailMessage msg, CancellationToken ct = default)
    {
        await _client.SendAsync(msg, ct);
        _log.LogInformation("Email sent to {To} subject {Subject}", msg.To, msg.Subject);
    }
}