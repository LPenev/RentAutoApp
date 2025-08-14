using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Data.Models;

public class SiteSetting
{
    public int Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public DateTime UpdatedOnUtc { get; set; } = DateTime.UtcNow;
}

