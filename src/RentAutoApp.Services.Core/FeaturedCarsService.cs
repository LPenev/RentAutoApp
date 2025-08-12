using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.ViewModels.Vehicles;

namespace RentAutoApp.Services.Core;

public class FeaturedCarsService : IFeaturedCarsService
{
    private readonly RentAutoAppDbContext _db;
    private readonly IMemoryCache _cache;

    public FeaturedCarsService(RentAutoAppDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<IReadOnlyList<FeaturedCarViewModel>> GetFeaturedAsync(int count, CancellationToken ct = default)
    {
        var cacheKey = $"featured:{count}";
        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<FeaturedCarViewModel> cached))
            return cached;

        var data = await _db.Vehicles
            .AsNoTracking()
            .Where(v => v.IsAvailable)
            .Include(v => v.Images)
            .OrderBy(v => v.PricePerDay)
            .Take(count)
            .Select(v => new FeaturedCarViewModel
            {
                Id = v.Id,
                Title = v.Brand + " " + v.Model,
                PricePerDay = v.PricePerDay,
                ImageUrl = v.Images.Select(i => i.ImageUrl).FirstOrDefault() ?? "/images/placeholder.png"
            })
            .ToListAsync(ct);

        _cache.Set(cacheKey, data, TimeSpan.FromMinutes(5));
        return data;
    }
}


