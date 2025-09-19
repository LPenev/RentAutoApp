using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Services.Messaging.Contracts;

namespace RentAutoApp.Web.Infrastructure.Email
{
    public static class EmailEndpoints
    {
        public static void MapEmailEndpoints(this IEndpointRouteBuilder app, bool emailSenderEnabled)
        {
            var sp = app.ServiceProvider;
            var env = sp.GetRequiredService<IHostEnvironment>();

            if (!env.IsDevelopment() || !emailSenderEnabled)
                return;

            app.MapGet("/dev/test-email", async (HttpContext httpContext) =>
                {
                    var opts = httpContext.RequestServices.GetRequiredService<IOptions<EmailTestOptions>>().Value;
                    var svc = httpContext.RequestServices.GetRequiredService<IEmailService>();

                    var to = httpContext.Request.Query["to"].FirstOrDefault() ?? opts.DefaultRecipient;
                    if (string.IsNullOrWhiteSpace(to))
                    {
                        return Results.BadRequest("No recipient configured for test email.");
                    }

                    try
                    {
                        await svc.SendAsync(new EmailMessage { To = to!, Subject = opts.Subject, HtmlBody = opts.BodyHtml });
                        return Results.Ok($"Sent to {to}");
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem($"Failed to send to {to}: {ex.Message}");
                    }
                })
                .WithTags("Dev")
                .WithName("Dev_TestEmail");
        }
    }
}