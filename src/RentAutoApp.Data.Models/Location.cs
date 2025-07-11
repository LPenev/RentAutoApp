namespace RentAutoApp.Data.Models;

public class Location
{
    public int Id { get; set; }

    public int CountryId { get; set; }

    public Country Country { get; set; } = null!;

    public int CityId { get; set; }

    public City City { get; set; } = null!;

    public int StreetId { get; set; }

    public Street Street { get; set; } = null!;
}
