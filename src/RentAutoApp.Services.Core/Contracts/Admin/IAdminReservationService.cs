using RentAutoApp.Services.Common;
using RentAutoApp.Web.ViewModels.Admin.Reservations;

namespace RentAutoApp.Services.Core.Contracts.Admin;

public interface IAdminReservationService
{
    Task<Result<List<ReservationAdminListItemViewModel>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> ConfirmAsync(int id, CancellationToken ct = default);
    Task<Result> MarkPickedUpAsync(int id, CancellationToken ct = default);
    Task<Result> MarkReturnedAsync(int id, CancellationToken ct = default);
    Task<Result> MarkExpiredAsync(int id, CancellationToken ct = default);
    Task<Result> MarkNoShowAsync(int id, CancellationToken ct = default);
    Task<Result> MarkPaidAsync(int id, bool isPaid, CancellationToken ct = default);
}
