using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.ViewModels.Contact;
using static RentAutoApp.GCommon.Constants.Contact;


namespace RentAutoApp.Web.Controllers;

public class ContactController : Controller
{
    private readonly IContactService _contactServce;

    public ContactController(IContactService contactServce) => _contactServce = contactServce;

    [HttpGet] 
    public IActionResult Index() => View(new ContactFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactFormViewModel vm, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(vm);
        await _contactServce.SendContactAsync(new ContactRequest(vm.Name, vm.Email, vm.Phone, vm.Subject, vm.Message), ct);
        TempData["ContactOk"] = LabelContactOK;
        return RedirectToAction(nameof(Index));
    }
}

