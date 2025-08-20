using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.ViewModels;

namespace RentAutoApp.Services.Core;

public sealed class ErrorPageService(ILogger<ErrorPageService> logger) : IErrorPageService
{
    public (string viewName, WebErrorViewModel model) GetForStatusCode(HttpContext http, int code)
    {
        var reexec = http.Features.Get<IStatusCodeReExecuteFeature>();

        var (title, message, view) = code switch
        {
            400 => ("Невалидна заявка", "Заявката не може да бъде обработена. Моля, проверете въведените данни и опитайте отново.", "400"),
            401 => ("Неоторизиран достъп", "За да видите тази страница, трябва да влезете в системата.", "401"),
            404 => ("Страницата не е намерена", "Линкът е невалиден или ресурсът липсва.", "404"),
            403 => ("Забранен достъп", "Достъпът е забранен.", "403"),
            410 => ("Линкът е изтекъл", "Линкът е изтекъл (Gone).", "410"),
            500 => ("Вътрешна грешка", "Възникна вътрешна грешка.", "500"),
            _ => ("Грешка", "Възникна грешка.", "Generic")
        };

        var model = new WebErrorViewModel
        {
            Code = code,
            Title = title,
            Message = message,
            OriginalPath = reexec?.OriginalPath,
            OriginalQuery = reexec?.OriginalQueryString
        };

        logger.LogWarning("HTTP {Code} at {Path}{Query}", code, model.OriginalPath, model.OriginalQuery);
        return (view, model);
    }

    public (string viewName, WebErrorViewModel model) GetForException(HttpContext http)
    {
        var ex = http.Features.Get<IExceptionHandlerPathFeature>();

        var model = new WebErrorViewModel
        {
            Code = 500,
            Title = "Вътрешна грешка",
            Message = "Опа! Нещо се обърка.",
            ExceptionPath = ex?.Path,
            ExceptionMessage = ex?.Error.Message
        };

        if (ex?.Error is not null)
            logger.LogError(ex.Error, "Unhandled exception at {Path}", ex.Path);

        return ("500", model);
    }
}
