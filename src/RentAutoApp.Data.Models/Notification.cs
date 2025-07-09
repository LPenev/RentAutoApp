namespace RentAutoApp.Data.Models;

public class Notification
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public int? VehicleId { get; set; }
    public int? ReservationId { get; set; }
    public int? DocumentId { get; set; }
    public int? ServiceId { get; set; }

    public string Type { get; set; } = null!;
    public string Message { get; set; } = null!;

    public DateTime ScheduledAt { get; set; }
    public DateTime? SentAt { get; set; }
    public bool IsRead { get; set; }
}
