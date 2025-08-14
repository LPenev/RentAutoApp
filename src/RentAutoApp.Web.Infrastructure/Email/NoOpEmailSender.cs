using Microsoft.AspNetCore.Identity.UI.Services;

namespace RentAutoApp.Web.Infrastructure.Email;

public sealed class NoOpEmailSender : IEmailSender
{
    public Task SendEmailAsync(string to, string subject, string html) => Task.CompletedTask;
}

