using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentAutoApp.Data.Models;
using RentAutoApp.Web.Data;

namespace RentAutoApp.Services.Core.Tests.Services
{
    [TestFixture]
    public class FeaturedCarsServiceTests
    {
        private static RentAutoAppDbContext NewDb(string name)
        {
            var options = new DbContextOptionsBuilder<RentAutoAppDbContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;

            return new RentAutoAppDbContext(options);
        }

        private static async Task SeedAsync(RentAutoAppDbContext db)
        {
            // Minimal related data for Vehicle foreign keys
            var country = new Country { Name = "Bulgaria" };
            var city = new City { Name = "Sofia", Country = country };
            var street = new Street { Name = "Vitosha", Number = "1", PostalCode = "1000", City = city };
            var location = new Location { Country = country, City = city, Street = street };
            var cat = new Category { Name = "Cars" };
            var sub = new SubCategory { Name = "SUV", Category = cat };
            db.Countries.Add(country);
            db.Cities.Add(city);
            db.Streets.Add(street);
            db.Locations.Add(location);
            db.Categories.Add(cat);
            db.SubCategories.Add(sub);

            db.Vehicles.AddRange(
                new Vehicle { Brand = "BMW", Model = "X5", RegistrationNumber = "CA0001", Vin = "VIN1", FirstRegistrationDate = DateTime.UtcNow, FuelType = RentAutoApp.GCommon.Enums.FuelType.Petrol, Transmission = RentAutoApp.GCommon.Enums.TransmissionType.Automatic, Seats = 5, Doors = 5, Mileage = 10000, PricePerDay = 150, PricePerHour = 15, IsAvailable = true, IsArchived = false, SubCategory = sub, Location = location, Images = { new VehicleImage { ImageUrl = "/img/bmw.jpg", UploadedAt = DateTime.UtcNow } } },
                new Vehicle { Brand = "Audi", Model = "A3", RegistrationNumber = "CA0002", Vin = "VIN2", FirstRegistrationDate = DateTime.UtcNow, FuelType = RentAutoApp.GCommon.Enums.FuelType.Diesel, Transmission = RentAutoApp.GCommon.Enums.TransmissionType.Manual, Seats = 5, Doors = 5, Mileage = 20000, PricePerDay = 90, PricePerHour = 9, IsAvailable = true, IsArchived = false, SubCategory = sub, Location = location, Images = { new VehicleImage { ImageUrl = "/img/audi.jpg", UploadedAt = DateTime.UtcNow } } },
                new Vehicle { Brand = "VW", Model = "Golf", RegistrationNumber = "CA0003", Vin = "VIN3", FirstRegistrationDate = DateTime.UtcNow, FuelType = RentAutoApp.GCommon.Enums.FuelType.Petrol, Transmission = RentAutoApp.GCommon.Enums.TransmissionType.Manual, Seats = 5, Doors = 5, Mileage = 30000, PricePerDay = 70, PricePerHour = 7, IsAvailable = false, IsArchived = false, SubCategory = sub, Location = location, Images = { new VehicleImage { ImageUrl = "/img/vw.jpg", UploadedAt = DateTime.UtcNow } } } // not available
            );

            await db.SaveChangesAsync();
        }

        [Test]
        public async Task GetFeaturedAsync_Returns_OnlyAvailable_OrderedByPrice_Takes_Count_And_Maps_VM()
        {
            using var db = NewDb(nameof(GetFeaturedAsync_Returns_OnlyAvailable_OrderedByPrice_Takes_Count_And_Maps_VM));
            await SeedAsync(db);
            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new FeaturedCarsService(db, cache);

            var list = await sut.GetFeaturedAsync(2);

            // Should exclude the unavailable (VW Golf) and take cheapest first
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0].Title, Is.EqualTo("Audi A3"));
            Assert.That(list[0].PricePerDay, Is.EqualTo(90));
            Assert.That(list[0].ImageUrl, Is.EqualTo("/img/audi.jpg"));

            Assert.That(list[1].Title, Is.EqualTo("BMW X5"));
            Assert.That(list[1].PricePerDay, Is.EqualTo(150));
            Assert.That(list[1].ImageUrl, Is.EqualTo("/img/bmw.jpg"));
        }

        [Test]
        public async Task GetFeaturedAsync_Uses_Cache_By_Count_Key()
        {
            using var db = NewDb(nameof(GetFeaturedAsync_Uses_Cache_By_Count_Key));
            await SeedAsync(db);
            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new FeaturedCarsService(db, cache);

            var first = await sut.GetFeaturedAsync(1);
            // Change DB to see if cache shields subsequent call
            var vehicle = await db.Vehicles.Where(v => v.IsAvailable).OrderBy(v => v.PricePerDay).FirstAsync();
            vehicle.PricePerDay = 1; // would change ordering if not cached
            await db.SaveChangesAsync();

            var second = await sut.GetFeaturedAsync(1);

            // Should be same as first due to 5min absolute cache with key featured:{count}
            Assert.That(second.Select(x => (x.Title, x.PricePerDay)).ToArray(),
                        Is.EqualTo(first.Select(x => (x.Title, x.PricePerDay)).ToArray()));
        }

        [Test]
        public async Task GetFeaturedAsync_Maps_Placeholder_Image_When_No_Images()
        {
            using var db = NewDb(nameof(GetFeaturedAsync_Maps_Placeholder_Image_When_No_Images));
            // Seed minimal relations
            var country = new Country { Name = "Bulgaria" };
            var city = new City { Name = "Sofia", Country = country };
            var street = new Street { Name = "Vitosha", Number = "2", PostalCode = "1000", City = city };
            var location = new Location { Country = country, City = city, Street = street };
            var cat = new Category { Name = "Cars" };
            var sub = new SubCategory { Name = "Sedan", Category = cat };
            db.Countries.Add(country);
            db.Cities.Add(city);
            db.Streets.Add(street);
            db.Locations.Add(location);
            db.Categories.Add(cat);
            db.SubCategories.Add(sub);

            db.Vehicles.Add(new Vehicle
            {
                Brand = "Skoda",
                Model = "Octavia",
                RegistrationNumber = "CA0004",
                Vin = "VIN4",
                FirstRegistrationDate = DateTime.UtcNow,
                FuelType = RentAutoApp.GCommon.Enums.FuelType.Petrol,
                Transmission = RentAutoApp.GCommon.Enums.TransmissionType.Manual,
                Seats = 5,
                Doors = 5,
                Mileage = 5000,
                PricePerDay = 80,
                PricePerHour = 8,
                IsAvailable = true,
                IsArchived = false,
                SubCategory = sub,
                Location = location
            });
            await db.SaveChangesAsync();

            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new FeaturedCarsService(db, cache);

            var list = await sut.GetFeaturedAsync(1);
            Assert.That(list.Single().ImageUrl, Is.EqualTo("/images/placeholder.jpg"));
        }
    }
}
