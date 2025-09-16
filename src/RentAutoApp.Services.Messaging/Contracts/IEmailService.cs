using RentAutoApp.Infrastructure.Email.Models;

namespace RentAutoApp.Services.Messaging.Contracts;

public interface IEmailService
{
    Task SendAsync(EmailMessage msg, CancellationToken ct = default);
}