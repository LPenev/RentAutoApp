using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RentAutoApp.Web.Models;
using System.Diagnostics;

namespace RentAutoApp.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStringLocalizer<SharedResource> L;
    public HomeController(
                ILogger<HomeController> logger,
                IStringLocalizer<SharedResource> localizer)
    {
        _logger = logger;
        L = localizer;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Terms()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
