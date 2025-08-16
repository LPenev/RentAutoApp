using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.Infrastructure.Contracts;
using RentAutoApp.Web.ViewModels.Reservations;

namespace RentAutoApp.Web.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly IReservationService _service;
    private readonly ICurrentUserService _current;

    public ReservationController(IReservationService service, ICurrentUserService current)
    {
        _service = service;
        _current = current;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var res = await _service.GetMyAsync(_current.UserId!, ct);
        return res.Succeeded ? View(res.Value) : Problem(res.Error);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int vehicleId, DateTime? startDate, DateTime? endDate, CancellationToken ct)
        => View(await _service.GetCreateModelAsync(vehicleId, startDate, endDate, ct));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationCreateInputModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await _service.PopulateLocationsAsync(model, ct);
            return View(model);
        }

        var res = await _service.CreateAsync(model, _current.UserId!, ct);
        if (!res.Succeeded)
        {
            ModelState.AddModelError(string.Empty, res.Error!);
            await _service.PopulateLocationsAsync(model, ct);
            return View(model);
        }

        TempData["Success"] = "Резервацията е създадена.";
        return RedirectToAction(nameof(Details), new { id = res.Value });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var res = await _service.GetDetailsAsync(id, _current.UserId!, _current.IsInRole("Admin"), ct);
        return res.Succeeded ? View(res.Value) : NotFound();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        var res = await _service.CancelAsync(id, _current.UserId!, _current.IsInRole("Admin"), ct);
        TempData[res.Succeeded ? "Success" : "Error"] = res.Succeeded ? "Резервацията е отменена." : (res.Error ?? "Грешка");
        return RedirectToAction(nameof(Index));
    }
}
