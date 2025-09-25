using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Web.Features.UserPanel;
using RentAutoApp.Web.ViewModels.UserPanel;

namespace RentAutoApp.Web.Areas.User.Controllers;

[Area("User")]
[Authorize]
//[Route("user/[controller]/[action]")]
//[Route("{culture:regex(^bg|de|en$)}/user/[controller]/[action]")]
public class ProfileController : Controller
{
    private readonly IUserPanelService _svc;

    public ProfileController(IUserPanelService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var res = await _svc.GetProfileAsync();
        if (!res.Succeeded) return BadRequest(res.Error);
        ViewBag.StatusMessage = TempData["StatusMessage"];
        return View(res.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Personal()
    {
        var res = await _svc.GetProfileAsync();
        if (!res.Succeeded) return BadRequest(res.Error);
        return View(res.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Personal(ProfileViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var res = await _svc.UpdatePersonalAsync(vm);
        if (!res.Succeeded)
        {
            ModelState.AddModelError(string.Empty, res.Error!);
            return View(vm);
        }
        TempData["StatusMessage"] = "Профилът е обновен.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Email() => View(new ChangeEmailViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Email(ChangeEmailViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var res = await _svc.ChangeEmailAsync(vm);
        if (!res.Succeeded)
        {
            ModelState.AddModelError(string.Empty, res.Error!);
            return View(vm);
        }
        TempData["StatusMessage"] = "Имейлът е сменен.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Password() => View(new ChangePasswordViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Password(ChangePasswordViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var res = await _svc.ChangePasswordAsync(vm);
        if (!res.Succeeded)
        {
            ModelState.AddModelError(string.Empty, res.Error!);
            return View(vm);
        }
        TempData["StatusMessage"] = "Паролата е сменена.";
        return RedirectToAction(nameof(Index));
    }
}
