namespace RentAutoApp.Data.Models;

public class Street
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public int CityId { get; set; }

    public City City { get; set; } = null!;
}
