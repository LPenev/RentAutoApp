﻿namespace RentAutoApp.Web.Infrastructure.Email;

public sealed class EmailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string From { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty; 
    public string Password { get; set; } = string.Empty;
}

