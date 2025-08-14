using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Contracts;

namespace RentAutoApp.Web.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
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
