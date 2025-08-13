using Microsoft.EntityFrameworkCore;
using RentAutoApp.GCommon.Enums;
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

    public async Task<List<VehicleListItemViewModel>> SearchAsync(
        int? locationId,
        int? subCategoryId,  // CarType
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken ct)
    {
        var q = _db.Vehicles
            .AsNoTracking()
            .Include(v => v.Images)
            .Include(v => v.SubCategory)
            .Include(v => v.Reservations)
            .AsQueryable();

        if (locationId.HasValue)
            q = q.Where(v => v.LocationId == locationId.Value);

        if (subCategoryId.HasValue)
            q = q.Where(v => v.SubCategoryId == subCategoryId.Value);

        // Filter free cars
        if (startDate.HasValue && endDate.HasValue)
        {
            var start = startDate.Value.Date;
            var end = endDate.Value.Date;

            q = q.Where(v => !v.Reservations.Any(r =>
                r.Status != ReservationStatus.Cancelled &&       // ignore canceled
                r.StartDate.Date <= end && r.EndDate.Date >= start));
        }

        
        return await q
            .OrderBy(v => v.PricePerDay)
            .Select(v => new VehicleListItemViewModel
            {
                Id = v.Id,
                Title = v.Brand,
                PricePerDay = v.PricePerDay,
                ImageUrl = v.Images
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault() ?? "/images/placeholder.jpg"
            })
            .ToListAsync(ct);
    }
}

