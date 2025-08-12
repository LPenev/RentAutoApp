using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Contracts;

namespace RentAutoApp.Web.ViewComponents;

public class FeaturedCarsViewComponent : ViewComponent
{
    private readonly IFeaturedCarsService _service;
    public FeaturedCarsViewComponent(IFeaturedCarsService service) => _service = service;

    public async Task<IViewComponentResult> InvokeAsync(int count = 3)
        => View(await _service.GetFeaturedAsync(count));
}

