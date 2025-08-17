using Microsoft.EntityFrameworkCore;
using RentAutoApp.Data.Models;
using RentAutoApp.GCommon.Enums;
using RentAutoApp.Web.Data;

namespace RentAutoApp.Services.Core.Tests.Services
{
    [TestFixture]
    public class VehicleServiceTests
    {
        private static RentAutoAppDbContext NewDb(string name)
        {
            var options = new DbContextOptionsBuilder<RentAutoAppDbContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;
            return new RentAutoAppDbContext(options);
        }

        private static async Task<(Vehicle v1, Vehicle v2, Location loc1, Location loc2, SubCategory carType1, SubCategory carType2)> SeedVehiclesAsync(RentAutoAppDbContext db)
        {
            var country = new Country { Name = "Bulgaria" };
            var city = new City { Name = "Sofia", Country = country };
            var street1 = new Street { Name = "Vitosha", Number = "1", PostalCode = "1000", City = city };
            var street2 = new Street { Name = "Graf", Number = "2", PostalCode = "1000", City = city };
            var loc1 = new Location { Country = country, City = city, Street = street1 };
            var loc2 = new Location { Country = country, City = city, Street = street2 };
            var cat = new Category { Name = "Cars" };
            var type1 = new SubCategory { Name = "SUV", Category = cat };
            var type2 = new SubCategory { Name = "Compact", Category = cat };

            db.Countries.Add(country);
            db.Cities.Add(city);
            db.Streets.AddRange(street1, street2);
            db.Locations.AddRange(loc1, loc2);
            db.Categories.Add(cat);
            db.SubCategories.AddRange(type1, type2);

            var v1 = new Vehicle
            {
                Brand = "BMW",
                Model = "X3",
                RegistrationNumber = "CA0001",
                Vin = "VIN1",
                FirstRegistrationDate = new DateTime(2022, 5, 1),
                FuelType = FuelType.Petrol,
                Transmission = TransmissionType.Automatic,
                Seats = 5,
                Doors = 5,
                Mileage = 15000,
                PricePerDay = 120,
                PricePerHour = 12,
                IsAvailable = true,
                IsArchived = false,
                SubCategory = type1,
                Location = loc1,
                Images = new List<VehicleImage> { new VehicleImage { ImageUrl = "/img/bmw.jpg", UploadedAt = DateTime.UtcNow } }
            };

            var v2 = new Vehicle
            {
                Brand = "Audi",
                Model = "A3",
                RegistrationNumber = "CA0002",
                Vin = "VIN2",
                FirstRegistrationDate = new DateTime(2021, 3, 1),
                FuelType = FuelType.Diesel,
                Transmission = TransmissionType.Manual,
                Seats = 5,
                Doors = 5,
                Mileage = 30000,
                PricePerDay = 80,
                PricePerHour = 8,
                IsAvailable = true,
                IsArchived = false,
                SubCategory = type2,
                Location = loc2,
                Images = new List<VehicleImage>() // no images to test placeholder
            };

            db.Vehicles.AddRange(v1, v2);
            await db.SaveChangesAsync();
            return (v1, v2, loc1, loc2, type1, type2);
        }

        [Test]
        public async Task GetVehicleDetailsAsync_ReturnsMappedDetails_OrNull()
        {
            using var db = NewDb(nameof(GetVehicleDetailsAsync_ReturnsMappedDetails_OrNull));
            var (v1, v2, loc1, loc2, type1, type2) = await SeedVehiclesAsync(db);
            var sut = new VehicleService(db);

            var details = await sut.GetVehicleDetailsAsync(v1.Id);
            Assert.That(details, Is.Not.Null);
            Assert.That(details!.Id, Is.EqualTo(v1.Id));
            Assert.That(details.Title, Is.EqualTo("BMW X3"));
            Assert.That(details.PricePerDay, Is.EqualTo(120));
            Assert.That(details.IsAvailable, Is.True);
            Assert.That(details.Brand, Is.EqualTo("BMW"));
            Assert.That(details.Model, Is.EqualTo("X3"));
            Assert.That(details.Year, Is.EqualTo(2022));
            Assert.That(details.Transmission, Is.EqualTo(TransmissionType.Automatic.ToString()));
            Assert.That(details.Fuel, Is.EqualTo(FuelType.Petrol.ToString()));
            Assert.That(details.ImageUrls, Contains.Item("/img/bmw.jpg"));

            var missing = await sut.GetVehicleDetailsAsync(99999);
            Assert.That(missing, Is.Null);
        }

        [Test]
        public async Task SearchAsync_Filters_By_Location_And_SubCategory_And_Availability_And_Dates()
        {
            using var db = NewDb(nameof(SearchAsync_Filters_By_Location_And_SubCategory_And_Availability_And_Dates));
            var (v1, v2, loc1, loc2, type1, type2) = await SeedVehiclesAsync(db);

            // Add an overlapping reservation for v2 to exclude it by date filter
            db.Reservations.Add(new Reservation
            {
                UserId = "u1",
                Vehicle = v2,
                PickupLocation = loc2,
                ReturnLocation = loc2,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                Status = ReservationStatus.Booked,
                PriceCalculated = 100,
                CreatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();

            var sut = new VehicleService(db);

            // Filter to only loc1 + type1 within date range that overlaps the v2 reservation (so v2 excluded)
            var list = await sut.SearchAsync(locationId: loc1.Id, subCategoryId: type1.Id,
                startDate: DateTime.Today.AddDays(1), endDate: DateTime.Today.AddDays(3), ct: default);

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.Single().Id, Is.EqualTo(v1.Id));
            Assert.That(list.Single().ImageUrl, Is.EqualTo("/img/bmw.jpg"));
        }

        [Test]
        public async Task SearchAsync_Returns_Placeholder_When_No_Images()
        {
            using var db = NewDb(nameof(SearchAsync_Returns_Placeholder_When_No_Images));
            var (v1, v2, loc1, loc2, type1, type2) = await SeedVehiclesAsync(db);
            var sut = new VehicleService(db);

            // no filters, should return both ordered by price ascending (v2 cheaper first)
            var list = await sut.SearchAsync(null, null, null, null, default);

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list.First().Id, Is.EqualTo(v2.Id));
            Assert.That(list.First().ImageUrl, Is.EqualTo("/images/placeholder.jpg")); // v2 has no images
        }
    }
}
