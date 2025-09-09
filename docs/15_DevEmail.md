# 15 – Dev Email Endpoint

🚧 **Development Only** – only available in Development + when EmailSender is enabled.

## Route
`GET /dev/test-email`

## Config (appsettings.Development.json)
```json
{
  "EmailTest": {
    "Enabled": true,
    "DefaultRecipient": "developer@example.com",
    "OverrideTo": "capture@example.com",
    "Subject": "Test email",
    "BodyHtml": "<b>It works!</b>"
  }
}
```

## Recipient Resolution Flow
1. БД (`Contact.RecipientEmail`)
2. Fallback: DefaultRecipient
3. Fallback (Debug): DevEmailDefaults
4. OverrideTo (if set)

## Response
- 200 OK – email sent
  - Header: `X-Email-Overridden: true` if there is override
- 400 Bad Request – no recipient or Disabled
- 404 Not Found – not in Development/disabled feature

