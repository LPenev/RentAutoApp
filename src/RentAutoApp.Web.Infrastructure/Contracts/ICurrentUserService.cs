namespace RentAutoApp.Web.Infrastructure.Contracts;

public interface ICurrentUserService
{
    string? UserId { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}
