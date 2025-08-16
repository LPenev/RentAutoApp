using Microsoft.EntityFrameworkCore;
using RentAutoApp.Data.Models;
using RentAutoApp.Services.Common;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Services.Core.Contracts.Admin;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.ViewModels.Admin.Reservations;

namespace RentAutoApp.Services.Core.Admin;

public class AdminReservationService : IAdminReservationService
{
    private readonly RentAutoAppDbContext _db;
    private readonly IReservationService _reservationCore;

    public AdminReservationService(RentAutoAppDbContext db, IReservationService reservationCore)
    {
        _db = db;
        _reservationCore = reservationCore;
    }

    public async Task<Result<List<ReservationAdminListItemViewModel>>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _db.Set<Reservation>()
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReservationAdminListItemViewModel
            {
                Id = r.Id,
                UserEmail = r.User.Email!,
                VehicleTitle = r.Vehicle.Brand + " " + r.Vehicle.Model,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status.ToString(),
                PriceCalculated = r.PriceCalculated,
                IsPaid = r.IsPaid
            })
            .ToListAsync(ct);

        return Result<List<ReservationAdminListItemViewModel>>.Success(list);
    }

    public Task<Result> ConfirmAsync(int id, CancellationToken ct = default)
        => _reservationCore.ConfirmAsync(id, userId: null, isAdmin: true, ct);

    public Task<Result> MarkPickedUpAsync(int id, CancellationToken ct = default)
        => _reservationCore.MarkPickedUpAsync(id, userId: null, isAdmin: true, ct);

    public Task<Result> MarkReturnedAsync(int id, CancellationToken ct = default)
        => _reservationCore.MarkReturnedAsync(id, userId: null, isAdmin: true, ct);

    public Task<Result> MarkExpiredAsync(int id, CancellationToken ct = default)
        => _reservationCore.MarkExpiredAsync(id, userId: null, isAdmin: true, ct);

    public Task<Result> MarkNoShowAsync(int id, CancellationToken ct = default)
        => _reservationCore.MarkNoShowAsync(id, userId: null, isAdmin: true, ct);

    public async Task<Result> MarkPaidAsync(int id, bool isPaid, CancellationToken ct = default)
    {
        var r = await _db.Set<Reservation>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (r is null) return Result.Fail("Резервацията не е намерена.");
        r.IsPaid = isPaid;
        await _db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
