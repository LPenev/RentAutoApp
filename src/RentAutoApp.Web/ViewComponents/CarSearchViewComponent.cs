using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.ViewModels.Search;

namespace RentAutoApp.Web.ViewComponents;

public class CarSearchViewComponent : ViewComponent
{
    private readonly ICarSearchService _service;
    public CarSearchViewComponent(ICarSearchService service) => _service = service;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        CarSearchViewModel model = await _service.GetSearchModelAsync(HttpContext.RequestAborted);

        var formVm = new CarSearchFormViewModel
        {
            SelectedLocationId = model.SelectedLocationId,
            SelectedCarTypeId = model.SelectedCarTypeId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Locations = model.Locations.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name }),
            CarTypes = model.CarTypes.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
        };

        return View(formVm);
    }
}

// local view for SelectListItem
internal sealed class CarSearchFormViewModel
{
    public int? SelectedLocationId { get; set; }
    public int? SelectedCarTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public IEnumerable<SelectListItem> Locations { get; set; }
    public IEnumerable<SelectListItem> CarTypes { get; set; }
}

