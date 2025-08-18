using Microsoft.EntityFrameworkCore;
using RentAutoApp.Data.Models;
using RentAutoApp.Services.Common;
using RentAutoApp.Services.Core.Admin.Contracts;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.ViewModels.Admin.Vehicles;

namespace RentAutoApp.Services.Core.Admin;

public class AdminVehicleService : IAdminVehicleService
{
    private readonly RentAutoAppDbContext _db;
    public AdminVehicleService(RentAutoAppDbContext db) => _db = db;

    public async Task<Result<int>> CreateAsync(VehicleCreateInputModel model, CancellationToken ct = default)
    {
        if (await _db.Set<Vehicle>().AnyAsync(v => v.RegistrationNumber == model.RegistrationNumber, ct))
            return Result<int>.Fail("Регистрационният номер вече съществува.");

        var v = new Vehicle
        {
            Brand = model.Brand.Trim(),
            Model = model.Model.Trim(),
            RegistrationNumber = model.RegistrationNumber.Trim(),
            Vin = model.Vin.Trim(),
            FirstRegistrationDate = model.FirstRegistrationDate,
            FuelType = model.FuelType,
            Transmission = model.Transmission,
            PowerHp = model.PowerHp,
            PricePerDay = model.PricePerDay,
            PricePerHour = model.PricePerHour,
            IsAvailable = model.IsAvailable,
            IsArchived = false,
            SubCategoryId = model.SubCategoryId,
            LocationId = model.LocationId
        };

        _db.Add(v);
        await _db.SaveChangesAsync(ct);
        return Result<int>.Success(v.Id);
    }

    public async Task<Result> SoftDeleteAsync(int id, CancellationToken ct = default)
    {
        var v = await _db.Set<Vehicle>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (v is null) return Result.Fail("Автомобилът не е намерен.");
        if (v.IsArchived) return Result.Success();
        v.IsArchived = true;
        v.IsAvailable = false;
        await _db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> RestoreAsync(int id, CancellationToken ct = default)
    {
        var v = await _db.Set<Vehicle>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (v is null) return Result.Fail("Автомобилът не е намерен.");
        if (!v.IsArchived) return Result.Success();
        v.IsArchived = false;
        await _db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result<List<VehicleAdminListItemViewModel>>> GetAllAsync(bool includeArchived = false, CancellationToken ct = default)
    {
        var q = _db.Set<Vehicle>().AsNoTracking();
        if (!includeArchived) q = q.Where(v => !v.IsArchived);

        var list = await q
            .OrderBy(v => v.Brand).ThenBy(v => v.Model)
            .Select(v => new VehicleAdminListItemViewModel
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                RegistrationNumber = v.RegistrationNumber,
                IsAvailable = v.IsAvailable,
                IsArchived = v.IsArchived,
                PricePerDay = v.PricePerDay
            })
            .ToListAsync(ct);

        return Result<List<VehicleAdminListItemViewModel>>.Success(list);
    }
}

