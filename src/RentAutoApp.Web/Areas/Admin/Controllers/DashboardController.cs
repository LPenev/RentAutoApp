using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RentAutoApp.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
[Route("admin/[controller]/[action]")]
public class DashboardController : Controller
{
    [HttpGet]
    public IActionResult Index() => View();
}