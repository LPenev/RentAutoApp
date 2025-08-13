using RentAutoApp.GCommon.Enums;

namespace RentAutoApp.Data.Models;

public class Vehicle
{
    public int Id { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string RegistrationNumber { get; set; } = null!;
    public string Vin { get; set; } = null!;
    public DateTime FirstRegistrationDate { get; set; }

    public FuelType FuelType { get; set; }
    public TransmissionType Transmission { get; set; }
    public int PowerHp { get; set; }
    public int Seats { get; set; }
    public int Doors { get; set; }
    public int TrunkCapacity { get; set; }
    public int Mileage { get; set; }

    public decimal PricePerDay { get; set; }
    public decimal PricePerHour { get; set; }
    public bool IsAvailable { get; set; }

    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; } = null!;

    public ICollection<VehicleImage> Images { get; set; } = new HashSet<VehicleImage>();
    public ICollection<RepairHistory> RepairHistories { get; set; } = new HashSet<RepairHistory>();
}
