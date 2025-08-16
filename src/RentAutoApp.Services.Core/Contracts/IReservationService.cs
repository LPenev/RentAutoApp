using RentAutoApp.Services.Common;
using RentAutoApp.Web.ViewModels.Reservations;
using RentAutoApp.GCommon.Enums;

namespace RentAutoApp.Services.Core.Contracts;

public interface IReservationService
{
    // Build model for Create + fill dropdowns
    Task<ReservationCreateInputModel> GetCreateModelAsync(int vehicleId, DateTime? startDate, DateTime? endDate, CancellationToken ct = default);
    Task PopulateLocationsAsync(ReservationCreateInputModel model, CancellationToken ct = default);

    // Commands/queries returning Result/Result<T>
    Task<Result<int>> CreateAsync(ReservationCreateInputModel model, string userId, CancellationToken ct = default);
    Task<Result<List<ReservationListItemViewModel>>> GetMyAsync(string userId, CancellationToken ct = default);
    Task<Result<ReservationDetailsViewModel>> GetDetailsAsync(int id, string userId, bool isAdmin = false, CancellationToken ct = default);
    Task<Result> CancelAsync(int id, string userId, bool isAdmin = false, CancellationToken ct = default);

    // Admin ops for status management
    Task<Result> ConfirmAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default);
    Task<Result> MarkPickedUpAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default);
    Task<Result> MarkReturnedAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default);
    Task<Result> MarkExpiredAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default);
    Task<Result> MarkNoShowAsync(int id, string? userId, bool isAdmin, CancellationToken ct = default);
}