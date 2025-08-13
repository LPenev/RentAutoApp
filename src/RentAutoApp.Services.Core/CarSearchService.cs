using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentAutoApp.Data;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.ViewModels.Search;

namespace RentAutoApp.Services.Core
{
    public class CarSearchService : ICarSearchService
    {
        private readonly RentAutoAppDbContext _db;
        private readonly IMemoryCache _cache;

        public CarSearchService(RentAutoAppDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<CarSearchViewModel> GetSearchModelAsync(CancellationToken ct = default)
        {
            CarSearchViewModel vm = new CarSearchViewModel()
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };

            vm.Locations = await _cache.GetOrCreateAsync("search:locations", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _db.Locations
                    .AsNoTracking()
                    .Include(l => l.Country)
                    .Include(l => l.City)
                    .Include(l => l.Street)
                    .OrderBy(l => l.Country.Name)
                    .ThenBy(l => l.City.Name)
                    .ThenBy(l => l.Street.Name)
                    .Select(l => new IdNameDto
                    {
                        Id = l.Id,
                        Name = $"{l.City.Name}, {l.Street.Name} {l.Street.Number}"
                    })
                    .ToListAsync(ct);
            });

            vm.CarTypes = await _cache.GetOrCreateAsync("search:cars_subcategories", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _db.SubCategories
                    .AsNoTracking()
                    .Where(s => s.Category.Name == "Cars")
                    .OrderBy(s => s.Name)
                    .Select(s => new IdNameDto
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToListAsync(ct);
            });

            return vm;
        }
    }
}