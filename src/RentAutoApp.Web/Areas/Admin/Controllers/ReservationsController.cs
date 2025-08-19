using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Admin.Contracts;

namespace RentAutoApp.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
[Route("admin/[controller]/[action]")]
public class ReservationsController : Controller
{
    private readonly IAdminReservationService _svc;
    public ReservationsController(IAdminReservationService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var res = await _svc.GetAllAsync(ct);
        if (!res.Succeeded) return BadRequest(res.Error);
        return View(res.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(int id, CancellationToken ct = default)
    {
        var res = await _svc.ConfirmAsync(id, ct);
        TempData["StatusMessage"] = res.Succeeded ? "Резервацията е потвърдена." : res.Error;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PickedUp(int id, CancellationToken ct = default)
    {
        var res = await _svc.MarkPickedUpAsync(id, ct);
        TempData["StatusMessage"] = res.Succeeded ? "Маркирано като взето." : res.Error;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Returned(int id, CancellationToken ct = default)
    {
        var res = await _svc.MarkReturnedAsync(id, ct);
        TempData["StatusMessage"] = res.Succeeded ? "Маркирано като върнато." : res.Error;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Expired(int id, CancellationToken ct = default)
    {
        var res = await _svc.MarkExpiredAsync(id, ct);
        TempData["StatusMessage"] = res.Succeeded ? "Маркирано като изтекло." : res.Error;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NoShow(int id, CancellationToken ct = default)
    {
        var res = await _svc.MarkNoShowAsync(id, ct);
        TempData["StatusMessage"] = res.Succeeded ? "Маркирано като неявил се." : res.Error;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Paid(int id, bool isPaid, CancellationToken ct = default)
    {
        var res = await _svc.MarkPaidAsync(id, isPaid, ct);
        TempData["StatusMessage"] = res.Succeeded ? (isPaid ? "Маркирано като платено." : "Маркирано като неплатено.") : res.Error;
        return RedirectToAction(nameof(Index));
    }
}
