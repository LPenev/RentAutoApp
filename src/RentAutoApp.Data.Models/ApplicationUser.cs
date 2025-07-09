using Microsoft.AspNetCore.Identity;

namespace RentAutoApp.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string Role { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
