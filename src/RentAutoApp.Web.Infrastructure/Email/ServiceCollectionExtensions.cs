using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using RentAutoApp.Infrastructure.Email;
using RentAutoApp.Infrastructure.Email.Contracts;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Services.Messaging;
using RentAutoApp.Services.Messaging.Contracts;

namespace RentAutoApp.Web.Infrastructure.Email;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmailTestOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddOptions<EmailTestOptions>()
                .Bind(configuration.GetSection("EmailTest"))
                .ValidateDataAnnotations();
        return services;
    }

    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration, bool emailSenderEnabled)
    {
        services
                        .AddOptions<EmailSettings>()
                        .Bind(configuration.GetSection("EmailSettings"))
                        .Validate(s => !string.IsNullOrWhiteSpace(s.Smtp.From), "EmailSettings:Smtp:From is required.")
                        .Validate(s => !string.IsNullOrWhiteSpace(s.Smtp.Host), "EmailSettings:Smtp:Host is required.")
                        .Validate(s => s.Smtp.Port is > 0 and <= 65535, "EmailSettings:Smtp:Port must be between 1 and 65535.")
                        .ValidateOnStart();

        if (emailSenderEnabled)
        {
            services.AddSingleton<IEmailClient, SmtpEmailClient>();
        }
        else
        {
            services.AddSingleton<IEmailClient, NoOpEmailClient>();
        }

        // Application-level service
        services.AddScoped<IEmailService, EmailService>();

        // Adapter for Identity UI
        services.AddScoped<IEmailSender, IdentityEmailSenderAdapter>();

        return services;
    }
}