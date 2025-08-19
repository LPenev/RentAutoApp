using Microsoft.AspNetCore.Mvc;
using RentAutoApp.Services.Core.Contracts;

namespace RentAutoApp.Web.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{code}")]
    public IActionResult HttpStatusCodeHandler(
        int code,
        [FromServices] IErrorPageService errorSvc)
    {
        var (view, model) = errorSvc.GetForStatusCode(HttpContext, code);
        return View(view, model);
    }

    [Route("Error/500")]
    public IActionResult ExceptionHandler(
        [FromServices] IErrorPageService errorSvc)
    {
        var (view, model) = errorSvc.GetForException(HttpContext);
        return View(view, model);
    }
}

