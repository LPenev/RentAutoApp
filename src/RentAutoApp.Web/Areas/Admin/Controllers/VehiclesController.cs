using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Admin.Contracts;
using RentAutoApp.Web.ViewModels.Admin.Vehicles;

namespace RentAutoApp.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
[Route("admin/[controller]/[action]")]
public class VehiclesController : Controller
{
    private readonly IAdminVehicleService _svc;
    public VehiclesController(IAdminVehicleService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Index(bool includeArchived = false, CancellationToken ct = default)
    {
        var res = await _svc.GetAllAsync(includeArchived, ct);
        if (!res.Succeeded) return BadRequest(res.Error);
        ViewBag.IncludeArchived = includeArchived;
        return View(res.Value);
    }

    [HttpGet]
    public IActionResult Create() => View(new VehicleCreateInputModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VehicleCreateInputModel model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return View(model);
        var res = await _svc.CreateAsync(model, ct);
        if (!res.Succeeded)
        {
            ModelState.AddModelError(string.Empty, res.Error!);
            return View(model);
        }
        TempData["StatusMessage"] = "Автомобилът е добавен.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SoftDelete(int id, CancellationToken ct = default)
    {
        var res = await _svc.SoftDeleteAsync(id, ct);
        TempData["StatusMessage"] = res.Succeeded ? "Автомобилът е архивиран." : res.Error;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id, CancellationToken ct = default)
    {
        var res = await _svc.RestoreAsync(id, ct);
        TempData["StatusMessage"] = res.Succeeded ? "Автомобилът е възстановен." : res.Error;
        return RedirectToAction(nameof(Index), new { includeArchived = true });
    }
}
