using Microsoft.Extensions.Logging;
using RentAutoApp.Services.Core.Contracts;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using static RentAutoApp.GCommon.Constants.Contact;

namespace RentAutoApp.Services.Core;

public sealed class ContactService : IContactService
{
    private readonly IEmailSender _emailSender;
    private readonly ISettingsService _settings;
    private readonly ILogger<ContactService> _logger;

    public ContactService(IEmailSender emailSender, ISettingsService settings, ILogger<ContactService> logger)
    { _emailSender = emailSender; _settings = settings; _logger = logger; }

    public async Task SendContactAsync(ContactRequest req, CancellationToken ct = default)
    {
        var recipient = await _settings.GetAsync("Contact.RecipientEmail", ct)
                        ?? "backup@email.com"; // fallback

        var subject = string.IsNullOrWhiteSpace(req.Subject) ? LabelNewRequestFromSite : req.Subject;
        var html = $@"
            <h3>Ново запитване</h3>
            <p><b>Име:</b> {WebUtility.HtmlEncode(req.Name)}</p>
            <p><b>Email:</b> {WebUtility.HtmlEncode(req.Email)}</p>
            <p><b>Телефон:</b> {WebUtility.HtmlEncode(req.Phone ?? "")}</p>
            <p><b>Съобщение:</b><br/>{WebUtility.HtmlEncode(req.Message).Replace("\n", "<br/>")}</p>";

        await _emailSender.SendEmailAsync(recipient, subject, html);
        _logger.LogInformation(LabelContactEmailSentTo, recipient);
    }
}

