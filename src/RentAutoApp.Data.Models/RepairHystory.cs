namespace RentAutoApp.Data.Models;

public class RepairHistory
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime RepairDate { get; set; }

    public int Odometer { get; set; }

    public string? RepairShop { get; set; }

    public decimal? Cost { get; set; }

    public string? Notes { get; set; }
}

