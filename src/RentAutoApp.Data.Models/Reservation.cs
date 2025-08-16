using RentAutoApp.GCommon.Enums;

namespace RentAutoApp.Data.Models;

public class Reservation
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public int PickupLocationId { get; set; }
    public Location PickupLocation { get; set; } = null!;

    public int ReturnLocationId { get; set; }
    public Location ReturnLocation { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ReservationStatus Status { get; set; }
    public decimal PriceCalculated { get; set; }

    public bool IsPaid { get; set; }

    public DateTime CreatedAt { get; set; }
}
