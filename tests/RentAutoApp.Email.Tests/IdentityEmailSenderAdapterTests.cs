using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Services.Messaging.Contracts;
using RentAutoApp.Web.Infrastructure.Email;

namespace RentAutoApp.Email.Tests;

[TestFixture]
public class IdentityEmailSenderAdapterTests
{
    private sealed class FakeEmailService : IEmailService
    {
        public EmailMessage? Last { get; private set; }
        public Task SendAsync(EmailMessage msg, CancellationToken ct = default)
        { Last = msg; return Task.CompletedTask; }
    }

    [Test]
    public async Task SendEmailAsync_Maps_To_EmailMessage_And_Uses_From_From_Settings()
    {
        // arrange
        var svc = new FakeEmailService();
        var opts = Options.Create(new EmailSettings
        {
            Smtp = new SmtpOptions
            {
                Host = "localhost",
                Port = 25,
                EnableSsl = false,
                From = "no-reply@rentauto.online"
            }
        });
        IEmailSender adapter = new IdentityEmailSenderAdapter(svc, opts);

        // act
        await adapter.SendEmailAsync("user@example.com", "Subject", "<p>Body</p>");

        // assert
        Assert.That(svc.Last, Is.Not.Null);
        Assert.That(svc.Last!.To, Is.EqualTo("user@example.com"));
        Assert.That(svc.Last.Subject, Is.EqualTo("Subject"));
        Assert.That(svc.Last.HtmlBody, Is.EqualTo("<p>Body</p>"));
        // From come from configuration, wen no override is.
        Assert.That(svc.Last.From, Is.EqualTo("no-reply@rentauto.online"));
    }
}