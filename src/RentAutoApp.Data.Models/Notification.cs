namespace RentAutoApp.Data.Models;

public class Notification
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public int? VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public int? ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public int? DocumentId { get; set; }
    public Document Document { get; set; } = null!;
    public int? ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    public string Type { get; set; } = null!;
    public string Message { get; set; } = null!;

    public DateTime ScheduledAt { get; set; }
    public DateTime? SentAt { get; set; }
    public bool IsRead { get; set; }
}
