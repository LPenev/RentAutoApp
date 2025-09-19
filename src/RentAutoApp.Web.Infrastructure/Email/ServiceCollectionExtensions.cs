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
        // Bind  validate options (fail fast на старт)
        services
        .AddOptions<EmailSettings>()
        .Bind(configuration.GetSection("EmailSettings"))
        .ValidateDataAnnotations()
        .Validate(s =>
        {
            if (s is null || s.Smtp is null) return false;
            if (string.IsNullOrWhiteSpace(s.Smtp.Host)) return false;
            if (string.IsNullOrWhiteSpace(s.Smtp.From)) return false;
            if (s.Smtp.EnableSsl && s.Smtp.Port == 25) return false; // типично SSL не е на 25
            if (!string.IsNullOrWhiteSpace(s.Smtp.User) && string.IsNullOrWhiteSpace(s.Smtp.Password)) return false;
            return true;
        }, "Invalid EmailSettings configuration")
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