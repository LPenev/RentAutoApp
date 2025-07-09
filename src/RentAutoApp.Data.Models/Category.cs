namespace RentAutoApp.Data.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<SubCategory> Subcategories { get; set; } = new HashSet<SubCategory>();

    public ICollection<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
}