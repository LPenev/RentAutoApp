using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace RentAutoApp.Web.Infrastructure.Email
{
    public static class DevEmailEndpoints
    {
        public static void MapDevEmailEndpoints(this IEndpointRouteBuilder app, bool emailSenderEnabled)
        {
            var sp = app.ServiceProvider;
            var env = sp.GetRequiredService<IHostEnvironment>();

            if (!emailSenderEnabled || !env.IsDevelopment())
            {
                return;
            }

            app.MapGet("/dev/test-email", async (
                HttpContext httpContext,
                IEmailSender sender,
                IOptions<EmailTestOptions> emailTestOptions) =>
            {
                var opts = emailTestOptions.Value;

                if (!opts.Enabled)
                {
                    return Results.BadRequest("Email test endpoint is disabled by configuration.");
                }

                var to = opts.DefaultRecipient;
                

                if (string.IsNullOrWhiteSpace(to))
                {
                    return Results.BadRequest("No recipient configured for test email.");
                }


                await sender.SendEmailAsync(to!, opts.Subject, opts.BodyHtml);

                //httpContext.Response.Headers.Append("X-Email-Overridden", "true");

                return Results.Ok($"Sent to {to}");
            })
            .WithTags("Dev")
            .WithName("Dev_TestEmail");
        }
    }
}