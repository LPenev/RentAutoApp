namespace RentAutoApp.Infrastructure.Email.Models;

public sealed class EmailAttachment
{
    /// <summary>Name of file</summary>
    public required string FileName { get; init; }

    /// <summary>МIME type (example: "application/pdf"). if is null, .NET try to detect.</summary>
    public string? ContentType { get; init; }

    /// <summary>File content.</summary>
    public required byte[] Content { get; init; }
}