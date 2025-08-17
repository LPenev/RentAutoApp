
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using RentAutoApp.GCommon;
using RentAutoApp.Services.Core.Contracts;

namespace RentAutoApp.Services.Core.Tests.Services
{
    [TestFixture]
    public class ContactServiceTests
    {
        // --- Fakes ---
        private sealed class FakeEmailSender : IEmailSender
        {
            public string? To { get; private set; }
            public string? Subject { get; private set; }
            public string? Html { get; private set; }
            public int SendCount { get; private set; }

            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                To = email;
                Subject = subject;
                Html = htmlMessage;
                SendCount++;
                return Task.CompletedTask;
            }
        }

        private sealed class FakeSettingsService : ISettingsService
        {
            private readonly Dictionary<string, string?> _storage = new();
            public void Set(string key, string? value) => _storage[key] = value;

            public Task<string?> GetAsync(string key, CancellationToken ct = default)
                => Task.FromResult(_storage.TryGetValue(key, out var v) ? v : null);

            public Task SetAsync(string key, string value, CancellationToken ct = default)
            { Set(key, value); return Task.CompletedTask; }
        }

        private sealed class TestLogger<T> : ILogger<T>
        {
            public LogLevel? LastLevel { get; private set; }
            public string? LastMessage { get; private set; }

            public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                LastLevel = logLevel;
                LastMessage = formatter(state, exception);
            }

            private sealed class NullScope : IDisposable
            {
                public static readonly NullScope Instance = new();
                public void Dispose() { }
            }
        }

        private static ContactRequest MakeReq(
            string name = "John <Doe>",
            string email = "john@example.com",
            string? phone = "123",
            string? subject = null,
            string message = "Hello\n<world>"
        ) => new(name, email, phone, subject, message);

        [Test]
        public async Task SendContactAsync_UsesRecipientFromSettings()
        {
            var email = new FakeEmailSender();
            var settings = new FakeSettingsService();
            var logger = new TestLogger<ContactService>();
            settings.Set("Contact.RecipientEmail", "to@example.com");
            var sut = new ContactService(email, settings, logger);

            await sut.SendContactAsync(MakeReq());

            Assert.That(email.SendCount, Is.EqualTo(1));
            Assert.That(email.To, Is.EqualTo("to@example.com"));
        }

        [Test]
        public async Task SendContactAsync_UsesFallbackRecipient_WhenSettingMissing()
        {
            var email = new FakeEmailSender();
            var settings = new FakeSettingsService(); // no key set
            var logger = new TestLogger<ContactService>();
            var sut = new ContactService(email, settings, logger);

            await sut.SendContactAsync(MakeReq());

            Assert.That(email.To, Is.EqualTo("backup@email.com"));
        }

        [Test]
        public async Task SendContactAsync_Subject_Defaults_WhenMissing()
        {
            var email = new FakeEmailSender();
            var settings = new FakeSettingsService();
            settings.Set("Contact.RecipientEmail", "to@example.com");
            var logger = new TestLogger<ContactService>();
            var sut = new ContactService(email, settings, logger);
            var req = MakeReq(subject: null);

            await sut.SendContactAsync(req);

            Assert.That(email.Subject, Is.EqualTo(Constants.Contact.LabelNewRequestFromSite));
        }

        [Test]
        public async Task SendContactAsync_Subject_UsesProvided()
        {
            var email = new FakeEmailSender();
            var settings = new FakeSettingsService();
            settings.Set("Contact.RecipientEmail", "to@example.com");
            var logger = new TestLogger<ContactService>();
            var sut = new ContactService(email, settings, logger);
            var req = MakeReq(subject: "Custom Subject");

            await sut.SendContactAsync(req);

            Assert.That(email.Subject, Is.EqualTo("Custom Subject"));
        }

        [Test]
        public async Task SendContactAsync_Html_EncodesFields_And_ReplacesNewlines()
        {
            var email = new FakeEmailSender();
            var settings = new FakeSettingsService();
            settings.Set("Contact.RecipientEmail", "to@example.com");
            var logger = new TestLogger<ContactService>();
            var sut = new ContactService(email, settings, logger);

            var req = MakeReq(
                name: "A&B <C>",
                email: "evil@example.com",
                phone: "<script>",
                subject: "Hi",
                message: "line1\nline2 & <b>"
            );

            await sut.SendContactAsync(req);

            Assert.That(email.Html, Does.Contain("A&amp;B &lt;C&gt;"));
            Assert.That(email.Html, Does.Contain("evil@example.com"));
            Assert.That(email.Html, Does.Contain("&lt;script&gt;"));
            Assert.That(email.Html, Does.Contain("line1<br/>line2 &amp; &lt;b&gt;"));
        }

        [Test]
        public async Task SendContactAsync_LogsRecipient()
        {
            var email = new FakeEmailSender();
            var settings = new FakeSettingsService();
            settings.Set("Contact.RecipientEmail", "boss@example.com");
            var logger = new TestLogger<ContactService>();
            var sut = new ContactService(email, settings, logger);

            await sut.SendContactAsync(MakeReq());

            Assert.That(logger.LastLevel, Is.EqualTo(LogLevel.Information));
            Assert.That(logger.LastMessage, Does.Contain("boss@example.com"));
        }
    }
}
