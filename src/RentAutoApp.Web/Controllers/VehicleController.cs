using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Contracts;
using Microsoft.Extensions.Localization;

namespace RentAutoApp.Web.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public VehicleController(IVehicleService vehicleService, IStringLocalizer<SharedResource> localizer)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            // Show all Vehicles
            var results = await _vehicleService.SearchAsync(null, null, null, null, ct);
            return View(results);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var model = await _vehicleService.GetVehicleDetailsAsync(id, ct);
            if (model is null) return RedirectToAction("Index");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            int? selectedLocationId,
            int? SelectedSubCategoryId,      // CarType
            DateTime? startDate,
            DateTime? endDate,
            CancellationToken ct)
        {

            var referer = Request.Headers["Referer"].ToString();

            if (!startDate.HasValue || !endDate.HasValue)
            {
                TempData["ErrorMessage"] = _localizer["Error_MissingDates"].Value;
                return Redirect(referer);
            }

            if (startDate.Value >= endDate.Value)
            {
                TempData["ErrorMessage"] = _localizer["Error_StartDateBeforeEndDate"].Value;
                return Redirect(referer);
            }

            var results = await _vehicleService.SearchAsync(
                selectedLocationId,
                SelectedSubCategoryId,
                startDate,
                endDate,
                ct);

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View("SearchResults", results);
        }

    }
}
