using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Web.Infrastructure.Email;

namespace RentAutoApp.Email.Tests;

[TestFixture]
public class EmailOptionsValidationTests
{
    private static ServiceProvider BuildProvider(Dictionary<string, string?> kv, bool emailSenderEnabled)
    {
        var cfg = new ConfigurationBuilder()
            .AddInMemoryCollection(kv!)
            .Build();

        var services = new ServiceCollection();
        services.AddEmailTestOptions(cfg);
        services.AddEmailSender(cfg, emailSenderEnabled);
        return services.BuildServiceProvider(validateScopes: true);
    }

    [Test]
    public void Invalid_Config_Should_Throw_On_Options_Access()
    {
        var kv = new Dictionary<string, string?>
        {
            ["EmailSettings:EmailSenderEnabled"] = "true",
            ["EmailSettings:Smtp:Host"] = "", // invalid
            ["EmailSettings:Smtp:Port"] = "25",
            ["EmailSettings:Smtp:EnableSsl"] = "false",
            ["EmailSettings:Smtp:From"] = "no-reply@rentauto.online"
        }
        ;

        using var sp = BuildProvider(kv, emailSenderEnabled: false);
        var options = sp.GetRequiredService<IOptions<EmailSettings>>();
        Assert.Throws<OptionsValidationException>(() =>
                {
                    _ = options.Value; // тригърва ValidateDataAnnotations  нашата Validate
                });
    }

    [Test]
    public void Valid_Config_Should_Pass()
    {
        var kv = new Dictionary<string, string?>
        {
            ["EmailSettings:EmailSenderEnabled"] = "true",
            ["EmailSettings:Smtp:Host"] = "smtp.example.com",
            ["EmailSettings:Smtp:Port"] = "587",
            ["EmailSettings:Smtp:EnableSsl"] = "true",
            ["EmailSettings:Smtp:From"] = "no-reply@rentauto.online",
            ["EmailSettings:Smtp:User"] = "user",
            ["EmailSettings:Smtp:Password"] = "pass"
        }
        ;

        using var sp = BuildProvider(kv, emailSenderEnabled: false);
        var options = sp.GetRequiredService<IOptions<EmailSettings>>();
        Assert.DoesNotThrow(() => { _ = options.Value; });
    }
}