using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.ViewModels;
using System.Globalization;

namespace RentAutoApp.Services.Core;

public sealed class ErrorPageService(ILogger<ErrorPageService> logger) : IErrorPageService
{
    public (string viewName, WebErrorViewModel model) GetForStatusCode(HttpContext http, int code)
    {
        var reexec = http.Features.Get<IStatusCodeReExecuteFeature>();

        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;


        var view = code switch
        {
            400 => "400",
            401 => "401",
            403 => "403",
            404 => "404",
            410 => "410",
            500 => "500",
            _ => "Generic"
        };

        var model = new WebErrorViewModel
        {
            Code = code,
            ResourceKeyPrefix = $"Error.{code}",
            OriginalPath = reexec?.OriginalPath,
            OriginalQuery = reexec?.OriginalQueryString,
            HomeUrl = $"/{culture}/",
            ContactUrl = $"/{culture}/Contact"
        };

        logger.LogWarning("HTTP {Code} at {Path}{Query}", code, model.OriginalPath, model.OriginalQuery);
        return (view, model);
    }

    public (string viewName, WebErrorViewModel model) GetForException(HttpContext http)
    {
        var ex = http.Features.Get<IExceptionHandlerPathFeature>();

        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        var model = new WebErrorViewModel
        {
            Code = 500,
            ExceptionPath = ex?.Path,
            ExceptionMessage = ex?.Error.Message,
            HomeUrl = $"/{culture}/",
            ContactUrl = $"/{culture}/Contact"
        };

        if (ex?.Error is not null)
            logger.LogError(ex.Error, "Unhandled exception at {Path}", ex.Path);

        return ("500", model);
    }
}
