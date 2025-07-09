namespace RentAutoApp.Data.Models;

public class Document
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public string DocType { get; set; } = null!; // e.g., Insurance, MOT
    public string DocumentUrl { get; set; } = null!;

    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public DateTime? NotificationSentAt { get; set; }
}
