namespace RentAutoApp.Web.ViewModels.Vehicles;

public class VehicleDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal PricePerDay { get; set; }
    public bool IsAvailable { get; set; }

    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string Transmission { get; set; } = null!;
    public string Fuel { get; set; } = null!;
    public int Seats { get; set; }

    public IReadOnlyList<string> ImageUrls { get; set; } = new List<string>();
}
