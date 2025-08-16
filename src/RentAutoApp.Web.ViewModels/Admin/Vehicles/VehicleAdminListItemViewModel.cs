namespace RentAutoApp.Web.ViewModels.Admin.Vehicles;

public class VehicleAdminListItemViewModel
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public bool IsArchived { get; set; }
    public decimal PricePerDay { get; set; }
}
