using Microsoft.AspNetCore.Identity;

namespace RentAutoApp.Data.Models;

public class DamageReport
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    public string Description { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;
    public DateTime ReportedAt { get; set; }

    // User who reported the damage
    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;
}
