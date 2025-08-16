namespace RentAutoApp.Web.ViewModels.Reservations;

public class ReservationListItemViewModel
{
    public int Id { get; set; }
    public string VehicleTitle { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PriceCalculated { get; set; }
}

