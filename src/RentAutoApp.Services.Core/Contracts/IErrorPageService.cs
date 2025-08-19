using Microsoft.AspNetCore.Http;
using RentAutoApp.Web.ViewModels;

namespace RentAutoApp.Services.Core.Contracts;

public interface IErrorPageService
{
    (string viewName, WebErrorViewModel model) GetForStatusCode(HttpContext http, int code);
    (string viewName, WebErrorViewModel model) GetForException(HttpContext http);
}

