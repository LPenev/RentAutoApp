using RentAutoApp.Web.ViewModels.Vehicles;

namespace RentAutoApp.Services.Core.Contracts;

public interface IVehicleService
{
    Task<VehicleDetailsViewModel?> GetVehicleDetailsAsync(int id, CancellationToken ct = default);
}

