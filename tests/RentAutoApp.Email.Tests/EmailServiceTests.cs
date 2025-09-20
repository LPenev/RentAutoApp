using Microsoft.Extensions.Logging;
using RentAutoApp.Infrastructure.Email.Contracts;
using RentAutoApp.Infrastructure.Email.Models;
using RentAutoApp.Services.Messaging;


namespace RentAutoApp.Email.Tests;

[TestFixture]
public class EmailServiceTests
{
    private sealed class FakeEmailClient : IEmailClient
    {
        public EmailMessage? LastMessage { get; private set; }
        public int Calls { get; private set; }
        public Task SendAsync(EmailMessage message, CancellationToken ct = default)
        {
            LastMessage = message;
            Calls++;
            return Task.CompletedTask;
        }
    }

    private sealed class TestLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = new();
        public IDisposable BeginScope<TState>(TState state) => NullDisposable.Instance;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            => Messages.Add(formatter(state, exception));
        private sealed class NullDisposable : IDisposable { public static readonly NullDisposable Instance = new(); public void Dispose() { } }
    }

    [Test]
    public async Task SendAsync_Delegates_To_IEmailClient_And_Logs()
    {
        // arrange
        var fakeClient = new FakeEmailClient();
        var logger = new TestLogger<EmailService>();
        var sut = new EmailService(fakeClient, logger);
        var msg = new EmailMessage
        {
            To = "client@example.com",
            Subject = "Hello",
            HtmlBody = "<b>Test</b>",
            Cc = new[] { "cc@example.com" },
            Bcc = new[] { "bcc@example.com" }
        }
        ;

        // act
        await sut.SendAsync(msg, default);

        // assert
        Assert.That(fakeClient.Calls, Is.EqualTo(1));
        Assert.That(fakeClient.LastMessage, Is.Not.Null);
        Assert.That(fakeClient.LastMessage!.To, Is.EqualTo("client@example.com"));
        Assert.That(logger.Messages.Any(m => m.Contains("Email sent to client@example.com")), Is.True);
    }
}