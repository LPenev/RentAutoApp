namespace RentAutoApp.Infrastructure.Email;

public sealed class EmailMessage
{
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string HtmlBody { get; init; }


    public string? From { get; init; }
    public string? ReplyTo { get; init; }
    public IReadOnlyCollection<string> Cc { get; init; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Bcc { get; init; } = Array.Empty<string>();
}