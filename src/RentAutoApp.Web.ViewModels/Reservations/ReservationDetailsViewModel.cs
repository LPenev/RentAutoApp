namespace RentAutoApp.Web.ViewModels.Reservations;

public class ReservationDetailsViewModel
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string VehicleTitle { get; set; } = string.Empty;
    public string PickupLocation { get; set; } = string.Empty;
    public string ReturnLocation { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PriceCalculated { get; set; }
    public DateTime CreatedAt { get; set; }
}

