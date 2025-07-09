using System;
namespace RentAutoApp.Data.Models;

public class VehicleImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public DateTime UploadedAt { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
