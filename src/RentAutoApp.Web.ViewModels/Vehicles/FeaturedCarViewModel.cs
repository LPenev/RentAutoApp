namespace RentAutoApp.Web.ViewModels.Vehicles;

public class FeaturedCarViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal PricePerDay { get; set; }
    public string ImageUrl { get; set; } = null!;
}

