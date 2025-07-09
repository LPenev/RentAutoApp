namespace RentAutoApp.Data.Models;

public class Service
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public string ServiceType { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ServiceDate { get; set; }
    public int MileageAtService { get; set; }

    public DateTime? NextServiceDate { get; set; }
    public int? NextServiceMileage { get; set; }
    public DateTime? NotificationSentAt { get; set; }
}
