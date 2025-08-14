using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.Data;

namespace RentAutoApp.Services.Core;

public sealed class SettingsService : ISettingsService
{
    private readonly RentAutoAppDbContext _db;
    private readonly IMemoryCache _cache;
    private static readonly MemoryCacheEntryOptions CacheOpts =
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) };

    public SettingsService(RentAutoAppDbContext db, IMemoryCache cache)
    { _db = db; _cache = cache; }

    public async Task<string?> GetAsync(string key, CancellationToken ct = default)
    {
        if (_cache.TryGetValue(key, out string? v)) return v;
        var val = await _db.SiteSettings.AsNoTracking()
            .Where(s => s.Key == key).Select(s => s.Value).FirstOrDefaultAsync(ct);
        _cache.Set(key, val, CacheOpts);
        return val;
    }

    public async Task SetAsync(string key, string value, CancellationToken ct = default)
    {
        var e = await _db.SiteSettings.FirstOrDefaultAsync(s => s.Key == key, ct);
        if (e is null) _db.SiteSettings.Add(new() { Key = key, Value = value, UpdatedOnUtc = DateTime.UtcNow });
        else { e.Value = value; e.UpdatedOnUtc = DateTime.UtcNow; }
        await _db.SaveChangesAsync(ct);
        _cache.Remove(key);
    }
}
