namespace RentAutoApp.Data.Models;

public class Discount
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;

    public double Percentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

}
