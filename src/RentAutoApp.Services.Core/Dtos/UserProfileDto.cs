namespace RentAutoApp.Services.Core.Dtos;

public sealed record UserProfileDto(
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber
);

