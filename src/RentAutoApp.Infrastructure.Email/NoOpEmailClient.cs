using RentAutoApp.Infrastructure.Email.Contracts;
using RentAutoApp.Infrastructure.Email.Models;

namespace RentAutoApp.Infrastructure.Email;

public sealed class NoOpEmailClient : IEmailClient
{
    public Task SendAsync(EmailMessage message, CancellationToken ct = default) => Task.CompletedTask;
}
