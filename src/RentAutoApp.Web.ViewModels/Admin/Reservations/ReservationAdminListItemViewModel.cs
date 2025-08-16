namespace RentAutoApp.Web.ViewModels.Admin.Reservations;

public class ReservationAdminListItemViewModel
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string VehicleTitle { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PriceCalculated { get; set; }
    public bool IsPaid { get; set; }
}

