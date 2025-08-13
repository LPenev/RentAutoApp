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

        public IActionResult Index()
        {
            return null;
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var model = await _vehicleService.GetVehicleDetailsAsync(id, ct);
            if (model is null) return RedirectToAction("Index");
            return View(model);
        }
    }
}
