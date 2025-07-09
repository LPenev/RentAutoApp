namespace RentAutoApp.Data.Models;

public class SubCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
