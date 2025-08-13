using Microsoft.EntityFrameworkCore;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.ViewModels.Vehicles;

namespace RentAutoApp.Services.Core;

public class VehicleService : IVehicleService
{
    private readonly RentAutoAppDbContext _db;

    public VehicleService(RentAutoAppDbContext db)
    {
        _db = db;
    }

    public async Task<VehicleDetailsViewModel?> GetVehicleDetailsAsync(int id, CancellationToken ct = default)
    {
        var v = await _db.Vehicles
            .Include(x => x.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (v == null) return null;

        return new VehicleDetailsViewModel
        {
            Id = v.Id,
            Title = $"{v.Brand} {v.Model}",
            PricePerDay = v.PricePerDay,
            IsAvailable = v.IsAvailable,
            Brand = v.Brand,
            Model = v.Model,
            Year = v.FirstRegistrationDate.Year,
            Transmission = v.Transmission.ToString(),
            Fuel = v.FuelType.ToString(),
            Seats = v.Seats,
            Doors = v.Doors,
            ImageUrls = v.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>()
        };
    }
}

