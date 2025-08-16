using Microsoft.EntityFrameworkCore;
using RentAutoApp.Data.Models;
using RentAutoApp.GCommon.Enums;
using RentAutoApp.Services.Common;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.ViewModels.Reservations;

namespace RentAutoApp.Services.Core;

public class ReservationService : IReservationService
{
    private readonly RentAutoAppDbContext _db;

    public ReservationService(RentAutoAppDbContext db) => _db = db;

    public async Task<ReservationCreateInputModel> GetCreateModelAsync(int vehicleId, DateTime? startDate, DateTime? endDate, CancellationToken ct = default)
    {
        var vehicle = await _db.Set<Vehicle>().FirstOrDefaultAsync(v => v.Id == vehicleId, ct)
            ?? throw new InvalidOperationException("Автомобила не е намерен.");

        var model = new ReservationCreateInputModel
        {
            VehicleId = vehicle.Id,
            VehicleTitle = vehicle.Brand + " " + vehicle.Model,
            PricePerDay = vehicle.PricePerDay,
            StartDate = startDate ?? DateTime.Today.AddDays(1),
            EndDate = endDate ?? DateTime.Today.AddDays(3),
            PickupLocationId = vehicle.LocationId,
            ReturnLocationId = vehicle.LocationId
        };

        await PopulateLocationsAsync(model, ct);
        return model;
    }

    public async Task PopulateLocationsAsync(ReservationCreateInputModel model, CancellationToken ct = default)
    {
        model.Locations = await _db.Set<Location>()
            .Include(l => l.City)
            .Include(l => l.Street)
            .Select(l => new LocationOptionViewModel
            {
                Id = l.Id,
                Text = l.City.Name + ", " + l.Street.Name
            })
            .ToListAsync(ct);
    }

    public async Task<Result<int>> CreateAsync(ReservationCreateInputModel model, string userId, CancellationToken ct = default)
    {
        var vehicle = await _db.Set<Vehicle>().FirstOrDefaultAsync(v => v.Id == model.VehicleId, ct);
        if (vehicle == null) return Result<int>.Fail("Автомобила не е намерен.");

        if (model.EndDate <= model.StartDate)
            return Result<int>.Fail("Крайната дата трябва да е след началната дата.");

        var overlaps = await _db.Set<Reservation>().AnyAsync(r =>
            r.VehicleId == model.VehicleId &&
            r.Status != ReservationStatus.Cancelled &&
            r.Status != ReservationStatus.Expired &&
            r.Status != ReservationStatus.Returned &&
            model.StartDate < r.EndDate &&
            model.EndDate > r.StartDate, ct);

        if (overlaps) return Result<int>.Fail("В избраните дати вече има резервация за този автомобил.");

        var totalDays = Math.Ceiling((model.EndDate - model.StartDate).TotalDays);
        if (totalDays <= 0) totalDays = 1;
        var price = (decimal)totalDays * vehicle.PricePerDay;

        var entity = new Reservation
        {
            UserId = userId,
            VehicleId = model.VehicleId,
            PickupLocationId = model.PickupLocationId,
            ReturnLocationId = model.ReturnLocationId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Status = ReservationStatus.Booked,
            PriceCalculated = price,
            CreatedAt = DateTime.UtcNow
        };

        _db.Add(entity);
        await _db.SaveChangesAsync(ct);
        return Result<int>.Success(entity.Id);
    }

    public async Task<Result<List<ReservationListItemViewModel>>> GetMyAsync(string userId, CancellationToken ct = default)
    {
        var list = await _db.Set<Reservation>()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReservationListItemViewModel
            {
                Id = r.Id,
                VehicleTitle = r.Vehicle.Brand + " " + r.Vehicle.Model,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status.ToString(),
                PriceCalculated = r.PriceCalculated
            })
            .ToListAsync(ct);

        return Result<List<ReservationListItemViewModel>>.Success(list);
    }

    public async Task<Result<ReservationDetailsViewModel>> GetDetailsAsync(int id, string userId, bool isAdmin = false, CancellationToken ct = default)
    {
        var q = _db.Set<Reservation>().Where(r => r.Id == id);
        if (!isAdmin) q = q.Where(r => r.UserId == userId);

        var vm = await q.Select(r => new ReservationDetailsViewModel
        {
            Id = r.Id,
            VehicleId = r.VehicleId,
            VehicleTitle = r.Vehicle.Brand + " " + r.Vehicle.Model,
            PickupLocation = r.PickupLocation.City.Name + ", " + r.PickupLocation.Street.Name,
            ReturnLocation = r.ReturnLocation.City.Name + ", " + r.ReturnLocation.Street.Name,
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            Status = r.Status.ToString(),
            PriceCalculated = r.PriceCalculated,
            CreatedAt = r.CreatedAt
        }).FirstOrDefaultAsync(ct);

        return vm is null
            ? Result<ReservationDetailsViewModel>.Fail("Резервацията не е намерена.")
            : Result<ReservationDetailsViewModel>.Success(vm);
    }

    public async Task<Result> CancelAsync(int id, string userId, bool isAdmin = false, CancellationToken ct = default)
    {
        var r = await _db.Set<Reservation>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (r == null) return Result.Fail("Резервацията не е намерена.");
        if (!isAdmin && r.UserId != userId) return Result.Fail("Нямате права за това.");
        if (r.Status is ReservationStatus.Cancelled or ReservationStatus.Returned) return Result.Fail("Вече е завършено.");

        r.Status = ReservationStatus.Cancelled;
        await _db.SaveChangesAsync(ct);
        return Result.Success();
    }

    // Аdmin status operations
    public Task<Result> ConfirmAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default)
        => ChangeStatusAsync(id, userId, ReservationStatus.Confirmed, isAdmin, ct);

    public Task<Result> MarkPickedUpAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default)
        => ChangeStatusAsync(id, userId, ReservationStatus.PickedUp, isAdmin, ct);

    public Task<Result> MarkReturnedAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default)
        => ChangeStatusAsync(id, userId, ReservationStatus.Returned, isAdmin, ct);

    public Task<Result> MarkExpiredAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default)
        => ChangeStatusAsync(id, userId, ReservationStatus.Expired, isAdmin, ct);

    public Task<Result> MarkNoShowAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default)
        => ChangeStatusAsync(id, userId, ReservationStatus.NoShow, isAdmin, ct);

    private async Task<Result> ChangeStatusAsync(int id, string? userId, ReservationStatus newStatus, bool isAdmin, CancellationToken ct)
    {
        var r = await _db.Set<Reservation>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (r == null) return Result.Fail("Резервацията не е намерена.");
        if (!isAdmin && r.UserId != userId) return Result.Fail("Нямате права за това.");
        if (r.Status is ReservationStatus.Cancelled or ReservationStatus.Returned)
            return Result.Fail("Резервацията вече е завършена.");

        r.Status = newStatus;
        await _db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
