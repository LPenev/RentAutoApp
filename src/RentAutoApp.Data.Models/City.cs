namespace RentAutoApp.Data.Models;

public class City
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CountryId { get; set; }

    public Country Country { get; set; } = null!;

    public ICollection<Street> Streets { get; set; } = new List<Street>();
}

