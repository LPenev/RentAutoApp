using System;
namespace RentAutoApp.Data.Models;

public class Contract
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    public string ContractPdfUrl { get; set; } = null!;
    public DateTime? SignedAt { get; set; }
}
