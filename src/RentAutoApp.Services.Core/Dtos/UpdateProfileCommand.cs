namespace RentAutoApp.Services.Core.Dtos;

public sealed record UpdateProfileCommand(
    string? FirstName,
    string? LastName,
    string? PhoneNumber
);