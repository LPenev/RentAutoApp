namespace RentAutoApp.Services.Core.Contracts;

public interface IContactService
{
    Task SendContactAsync(ContactRequest req, CancellationToken ct = default);
}

public sealed record ContactRequest(string Name, string Email, string? Phone, string? Subject, string Message);
