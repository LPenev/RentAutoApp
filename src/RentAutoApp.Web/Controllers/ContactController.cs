using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.ViewModels.Contact;
using static RentAutoApp.GCommon.Constants;


namespace RentAutoApp.Web.Controllers;

public class ContactController : Controller
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService) => _contactService = contactService;

    [HttpGet]
    public IActionResult Index() => View(new ContactFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactFormViewModel vm, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(vm);

        await _contactService.SendContactAsync(new ContactRequest(vm.Name, vm.Email, vm.Phone, vm.Subject, vm.Message), ct);

        TempData["ContactOk"] = true;

        var culture = (string?)RouteData.Values["culture"] ?? HttpContext.Features
            .Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>()?
            .RequestCulture.Culture.TwoLetterISOLanguageName ?? DefaultCulture;

        return RedirectToAction(nameof(Index), new { culture });
    }
}

