namespace RentAutoApp.Data.Models;

public class Invoice
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    public string InvoicePdfUrl { get; set; } = null!;
    public DateTime IssuedAt { get; set; }
    public DateTime? SentToUserAt { get; set; }
}
