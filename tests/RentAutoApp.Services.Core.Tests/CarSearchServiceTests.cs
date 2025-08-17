
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentAutoApp.Data.Models;
using RentAutoApp.Web.Data;

namespace RentAutoApp.Services.Core.Tests.Services
{
    [TestFixture]
    public class CarSearchServiceTests
    {
        private static RentAutoAppDbContext NewDb(string name)
        {
            var options = new DbContextOptionsBuilder<RentAutoAppDbContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;

            return new RentAutoAppDbContext(options);
        }

        private static async Task SeedBasicAsync(RentAutoAppDbContext db)
        {
            var bg = new Country { Name = "Bulgaria" };
            var gr = new Country { Name = "Greece" };
            db.Countries.AddRange(bg, gr);

            var sofia = new City { Name = "Sofia", Country = bg };
            var athens = new City { Name = "Athens", Country = gr };
            db.Cities.AddRange(sofia, athens);

            var vitosha = new Street { Name = "Vitosha", Number = "15", PostalCode = "1000", City = sofia };
            var ermou = new Street { Name = "Ermou", Number = "10", PostalCode = "10563", City = athens };
            db.Streets.AddRange(vitosha, ermou);

            db.Locations.AddRange(
                new Location { Country = bg, City = sofia, Street = vitosha },
                new Location { Country = gr, City = athens, Street = ermou }
            );

            var cars = new Category { Name = "Cars" };
            var vans = new Category { Name = "Vans" };
            db.Categories.AddRange(cars, vans);

            db.SubCategories.AddRange(
                new SubCategory { Name = "SUV", Category = cars },
                new SubCategory { Name = "Compact", Category = cars },
                new SubCategory { Name = "Cargo", Category = vans }
            );

            await db.SaveChangesAsync();
        }

        [Test]
        public async Task GetSearchModelAsync_Sets_Default_Dates_And_Returns_Locations_And_CarTypes()
        {
            using var db = NewDb(nameof(GetSearchModelAsync_Sets_Default_Dates_And_Returns_Locations_And_CarTypes));
            await SeedBasicAsync(db);

            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new CarSearchService(db, cache);

            var vm = await sut.GetSearchModelAsync();

            Assert.That(vm, Is.Not.Null);
            Assert.That(vm.StartDate, Is.EqualTo(DateTime.Today));
            Assert.That(vm.EndDate, Is.EqualTo(DateTime.Today.AddDays(1)));

            var locs = vm.Locations.ToList();
            Assert.That(locs.Count, Is.EqualTo(2));
            Assert.That(locs[0].Name, Is.EqualTo("Sofia, Vitosha 15")); // Bulgaria first by country, then city/street
            Assert.That(locs[1].Name, Is.EqualTo("Athens, Ermou 10"));

            var carTypes = vm.CarTypes.ToList();
            Assert.That(carTypes.Select(c => c.Name).ToArray(), Is.EqualTo(new[] { "Compact", "SUV" })); // ordered by Name
        }

        [Test]
        public async Task GetSearchModelAsync_Uses_Cache_Between_Calls()
        {
            using var db = NewDb(nameof(GetSearchModelAsync_Uses_Cache_Between_Calls));
            await SeedBasicAsync(db);

            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new CarSearchService(db, cache);

            var vm1 = await sut.GetSearchModelAsync();
            var locs1 = vm1.Locations.ToList();
            var cars1 = vm1.CarTypes.ToList();

            // modify DB after first call
            var bg = await db.Countries.FirstAsync(c => c.Name == "Bulgaria");
            var plovdiv = new City { Name = "Plovdiv", Country = bg };
            var street6 = new Street { Name = "6th Septemvri", Number = "1", PostalCode = "4000", City = plovdiv };
            db.Cities.Add(plovdiv);
            db.Streets.Add(street6);
            db.Locations.Add(new Location { Country = bg, City = plovdiv, Street = street6 });

            var carsCat = await db.Categories.FirstAsync(c => c.Name == "Cars");
            db.SubCategories.Add(new SubCategory { Name = "Cabrio", Category = carsCat });
            await db.SaveChangesAsync();

            var vm2 = await sut.GetSearchModelAsync();
            var locs2 = vm2.Locations.ToList();
            var cars2 = vm2.CarTypes.ToList();

            // should still be old counts due to cache (10 min absolute)
            Assert.That(locs2.Count, Is.EqualTo(locs1.Count), "Locations should come from cache");
            Assert.That(cars2.Count, Is.EqualTo(cars1.Count), "Car types should come from cache");
        }

        [Test]
        public async Task GetSearchModelAsync_Filters_CarTypes_By_Cars_Category()
        {
            using var db = NewDb(nameof(GetSearchModelAsync_Filters_CarTypes_By_Cars_Category));
            await SeedBasicAsync(db);

            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new CarSearchService(db, cache);

            var vm = await sut.GetSearchModelAsync();

            Assert.That(vm.CarTypes.All(c => c.Name == "Compact" || c.Name == "SUV"), Is.True);
            Assert.That(vm.CarTypes.Any(c => c.Name == "Cargo"), Is.False);
        }
    }
}

