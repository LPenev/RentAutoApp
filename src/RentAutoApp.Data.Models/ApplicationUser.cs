using Microsoft.AspNetCore.Identity;

namespace RentAutoApp.Data.Models;


public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public string Role { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? LocationId { get; set; }

    public Location? Location { get; set; }
}
