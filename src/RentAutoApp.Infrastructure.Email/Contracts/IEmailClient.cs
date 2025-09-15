using RentAutoApp.Infrastructure.Email.Models;

namespace RentAutoApp.Infrastructure.Email.Contracts;

public interface IEmailClient
{
    Task SendAsync(EmailMessage message, CancellationToken ct = default);
}
