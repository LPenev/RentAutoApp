using RentAutoApp.Services.Common;
using RentAutoApp.Web.ViewModels.Admin.Vehicles;

namespace RentAutoApp.Services.Core.Admin.Contracts;

public interface IAdminVehicleService
{
    Task<Result<int>> CreateAsync(VehicleCreateInputModel model, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(int id, CancellationToken ct = default);
    Task<Result> RestoreAsync(int id, CancellationToken ct = default);
    Task<Result<List<VehicleAdminListItemViewModel>>> GetAllAsync(bool includeArchived = false, CancellationToken ct = default);
}
