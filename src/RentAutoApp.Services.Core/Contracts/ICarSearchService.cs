using RentAutoApp.Web.ViewModels.Search;

namespace RentAutoApp.Services.Core.Contracts;

public interface ICarSearchService
{
    Task<CarSearchViewModel> GetSearchModelAsync(CancellationToken ct = default);
}