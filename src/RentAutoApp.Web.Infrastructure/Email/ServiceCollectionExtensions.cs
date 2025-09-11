using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
}
