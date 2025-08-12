using RentAutoApp.Web.ViewModels.Vehicles;

namespace RentAutoApp.Services.Core.Contracts;

public interface IFeaturedCarsService
{
    Task<IReadOnlyList<FeaturedCarViewModel>> GetFeaturedAsync(int count, CancellationToken ct = default);
}

