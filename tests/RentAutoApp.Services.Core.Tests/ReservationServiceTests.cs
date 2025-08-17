using Microsoft.EntityFrameworkCore;
using RentAutoApp.Data.Models;
using RentAutoApp.GCommon.Enums;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.ViewModels.Reservations;

namespace RentAutoApp.Services.Core.Tests.Services
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private static RentAutoAppDbContext NewDb(string name)
        {
            var options = new DbContextOptionsBuilder<RentAutoAppDbContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;

            return new RentAutoAppDbContext(options);
        }

        private static async Task<(Vehicle vehicle, Location loc1, Location loc2)> SeedVehicleAsync(RentAutoAppDbContext db, decimal pricePerDay = 100m)
        {
            // Lookup data
            var country = new Country { Name = "Bulgaria" };
            var city = new City { Name = "Sofia", Country = country };
            var street1 = new Street { Name = "Vitosha", Number = "1", PostalCode = "1000", City = city };
            var street2 = new Street { Name = "Graf", Number = "2", PostalCode = "1000", City = city };
            var loc1 = new Location { Country = country, City = city, Street = street1 };
            var loc2 = new Location { Country = country, City = city, Street = street2 };
            var cat = new Category { Name = "Cars" };
            var sub = new SubCategory { Name = "Compact", Category = cat };

            db.Countries.Add(country);
            db.Cities.Add(city);
            db.Streets.AddRange(street1, street2);
            db.Locations.AddRange(loc1, loc2);
            db.Categories.Add(cat);
            db.SubCategories.Add(sub);

            var v = new Vehicle
            {
                Brand = "Audi",
                Model = "A3",
                RegistrationNumber = "CA0001",
                Vin = "VIN1",
                FirstRegistrationDate = DateTime.UtcNow,
                FuelType = RentAutoApp.GCommon.Enums.FuelType.Petrol,
                Transmission = RentAutoApp.GCommon.Enums.TransmissionType.Manual,
                Seats = 5,
                Doors = 5,
                Mileage = 10000,
                PricePerDay = pricePerDay,
                PricePerHour = 10,
                IsAvailable = true,
                IsArchived = false,
                SubCategory = sub,
                Location = loc1
            };
            db.Vehicles.Add(v);
            await db.SaveChangesAsync();
            return (v, loc1, loc2);
        }

        private static ReservationCreateInputModel MakeModel(int vehicleId, int pickupLocId, int returnLocId, DateTime start, DateTime end)
            => new()
            {
                VehicleId = vehicleId,
                PickupLocationId = pickupLocId,
                ReturnLocationId = returnLocId,
                StartDate = start,
                EndDate = end
            };

        [Test]
        public async Task CreateAsync_Fails_WhenVehicleMissing()
        {
            using var db = NewDb(nameof(CreateAsync_Fails_WhenVehicleMissing));
            var sut = new ReservationService(db);
            var model = MakeModel(vehicleId: 999, 1, 1, DateTime.Today, DateTime.Today.AddDays(1));

            var result = await sut.CreateAsync(model, userId: "u1");

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Error, Does.Contain("Автомобила не е намерен"));
        }

        [Test]
        public async Task CreateAsync_Fails_WhenEndNotAfterStart()
        {
            using var db = NewDb(nameof(CreateAsync_Fails_WhenEndNotAfterStart));
            var (v, loc1, loc2) = await SeedVehicleAsync(db);
            var sut = new ReservationService(db);
            var model = MakeModel(v.Id, loc1.Id, loc2.Id, DateTime.Today, DateTime.Today);

            var result = await sut.CreateAsync(model, "u1");

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Error, Does.Contain("Крайната дата трябва да е след"));
        }

        [Test]
        public async Task CreateAsync_Fails_WhenOverlaps_WithActiveReservation()
        {
            using var db = NewDb(nameof(CreateAsync_Fails_WhenOverlaps_WithActiveReservation));
            var (v, loc1, loc2) = await SeedVehicleAsync(db);
            // Existing reservation for same vehicle that overlaps
            db.Reservations.Add(new Reservation
            {
                UserId = "u2",
                Vehicle = v,
                PickupLocation = loc1,
                ReturnLocation = loc2,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                Status = ReservationStatus.Booked,
                PriceCalculated = 300,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            });
            await db.SaveChangesAsync();
            var sut = new ReservationService(db);

            var model = MakeModel(v.Id, loc1.Id, loc2.Id, DateTime.Today.AddDays(1), DateTime.Today.AddDays(4));
            var result = await sut.CreateAsync(model, "u1");

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Error, Does.Contain("вече има резервация"));
        }

        [Test]
        public async Task CreateAsync_Ignores_Overlaps_For_CancelledExpiredReturned()
        {
            using var db = NewDb(nameof(CreateAsync_Ignores_Overlaps_For_CancelledExpiredReturned));
            var (v, loc1, loc2) = await SeedVehicleAsync(db);

            foreach (var status in new[] { ReservationStatus.Cancelled, ReservationStatus.Expired, ReservationStatus.Returned })
            {
                db.Reservations.Add(new Reservation
                {
                    UserId = "u2",
                    Vehicle = v,
                    PickupLocation = loc1,
                    ReturnLocation = loc2,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(3),
                    Status = status,
                    PriceCalculated = 100,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                });
            }
            await db.SaveChangesAsync();

            var sut = new ReservationService(db);
            var model = MakeModel(v.Id, loc1.Id, loc2.Id, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var result = await sut.CreateAsync(model, "u1");

            Assert.That(result.Succeeded, Is.True, result.Error);
            // Verify inserted row
            var saved = await db.Reservations.OrderByDescending(r => r.Id).FirstAsync();
            Assert.That(saved.Status, Is.EqualTo(ReservationStatus.Booked));
        }

        [Test]
        public async Task CreateAsync_Computes_Price_Using_CeilDays_Min1()
        {
            using var db = NewDb(nameof(CreateAsync_Computes_Price_Using_CeilDays_Min1));
            var (v, loc1, loc2) = await SeedVehicleAsync(db, pricePerDay: 50m);
            var sut = new ReservationService(db);

            // same day 5 hours -> ceil to 1 day
            var start = DateTime.Today.AddHours(10);
            var end = DateTime.Today.AddHours(15);
            var result = await sut.CreateAsync(MakeModel(v.Id, loc1.Id, loc2.Id, start, end), "u1");
            Assert.That(result.Succeeded, Is.True, result.Error);
            var r1 = await db.Reservations.FindAsync(result.Value);
            Assert.That(r1!.PriceCalculated, Is.EqualTo(50m));

            // 1.2 days -> ceil to 2 days
            db.Reservations.RemoveRange(db.Reservations);
            await db.SaveChangesAsync();
            var start2 = DateTime.Today.AddHours(0);
            var end2 = DateTime.Today.AddDays(1).AddHours(6); // 30h => ceil 2 days
            var result2 = await sut.CreateAsync(MakeModel(v.Id, loc1.Id, loc2.Id, start2, end2), "u1");
            Assert.That(result2.Succeeded, Is.True, result2.Error);
            var r2 = await db.Reservations.FindAsync(result2.Value);
            Assert.That(r2!.PriceCalculated, Is.EqualTo(100m));
        }

        [Test]
        public async Task GetMyAsync_Returns_OnlyUser_OrderedByCreatedDesc()
        {
            using var db = NewDb(nameof(GetMyAsync_Returns_OnlyUser_OrderedByCreatedDesc));
            var (v, loc1, loc2) = await SeedVehicleAsync(db);

            db.Reservations.AddRange(
                new Reservation { UserId = "me", Vehicle = v, PickupLocation = loc1, ReturnLocation = loc2, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1), Status = ReservationStatus.Booked, PriceCalculated = 1, CreatedAt = new DateTime(2025, 1, 2) },
                new Reservation { UserId = "me", Vehicle = v, PickupLocation = loc1, ReturnLocation = loc2, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1), Status = ReservationStatus.Booked, PriceCalculated = 1, CreatedAt = new DateTime(2025, 1, 3) },
                new Reservation { UserId = "other", Vehicle = v, PickupLocation = loc1, ReturnLocation = loc2, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1), Status = ReservationStatus.Booked, PriceCalculated = 1, CreatedAt = new DateTime(2025, 1, 4) }
            );
            await db.SaveChangesAsync();

            var sut = new ReservationService(db);
            var res = await sut.GetMyAsync("me");

            Assert.That(res.Succeeded, Is.True);
            Assert.That(res.Value!.Count, Is.EqualTo(2));
            Assert.That(res.Value![0].StartDate, Is.EqualTo(DateTime.Today)); // maps fields
            // ensure order by CreatedAt desc (second added has later CreatedAt)
            Assert.That(res.Value![0].EndDate, Is.EqualTo(DateTime.Today.AddDays(1)));
        }

        [Test]
        public async Task GetDetailsAsync_Honors_Permissions()
        {
            using var db = NewDb(nameof(GetDetailsAsync_Honors_Permissions));
            var (v, loc1, loc2) = await SeedVehicleAsync(db);

            var myRes = new Reservation { UserId = "me", Vehicle = v, PickupLocation = loc1, ReturnLocation = loc2, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2), Status = ReservationStatus.Booked, PriceCalculated = 200, CreatedAt = DateTime.UtcNow };
            var otherRes = new Reservation { UserId = "other", Vehicle = v, PickupLocation = loc1, ReturnLocation = loc2, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2), Status = ReservationStatus.Booked, PriceCalculated = 200, CreatedAt = DateTime.UtcNow };
            db.Reservations.AddRange(myRes, otherRes);
            await db.SaveChangesAsync();

            var sut = new ReservationService(db);

            var ok = await sut.GetDetailsAsync(myRes.Id, "me", isAdmin: false);
            Assert.That(ok.Succeeded, Is.True);
            Assert.That(ok.Value!.VehicleTitle, Does.Contain("Audi A3"));

            var forbidden = await sut.GetDetailsAsync(otherRes.Id, "me", isAdmin: false);
            Assert.That(forbidden.Succeeded, Is.False);
        }

        [Test]
        public async Task CancelAsync_Sets_Status_To_Cancelled_ForOwner()
        {
            using var db = NewDb(nameof(CancelAsync_Sets_Status_To_Cancelled_ForOwner));
            var (v, loc1, loc2) = await SeedVehicleAsync(db);
            var r = new Reservation { UserId = "me", Vehicle = v, PickupLocation = loc1, ReturnLocation = loc2, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1), Status = ReservationStatus.Booked, PriceCalculated = 1, CreatedAt = DateTime.UtcNow };
            db.Reservations.Add(r);
            await db.SaveChangesAsync();

            var sut = new ReservationService(db);
            var result = await sut.CancelAsync(r.Id, "me", isAdmin: false);

            Assert.That(result.Succeeded, Is.True);
            Assert.That((await db.Reservations.FindAsync(r.Id))!.Status, Is.EqualTo(ReservationStatus.Cancelled));
        }

        [Test]
        public async Task CancelAsync_Fails_When_NotOwner_And_NotAdmin()
        {
            using var db = NewDb(nameof(CancelAsync_Fails_When_NotOwner_And_NotAdmin));
            var (v, loc1, loc2) = await SeedVehicleAsync(db);
            var r = new Reservation { UserId = "owner", Vehicle = v, PickupLocation = loc1, ReturnLocation = loc2, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1), Status = ReservationStatus.Booked, PriceCalculated = 1, CreatedAt = DateTime.UtcNow };
            db.Reservations.Add(r);
            await db.SaveChangesAsync();

            var sut = new ReservationService(db);
            var result = await sut.CancelAsync(r.Id, "intruder", isAdmin: false);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Error, Does.Contain("Нямате права"));
        }

        [Test]
        public async Task ConfirmAsync_Allows_Admin_To_Change_Status()
        {
            using var db = NewDb(nameof(ConfirmAsync_Allows_Admin_To_Change_Status));
            var (v, loc1, loc2) = await SeedVehicleAsync(db);
            var r = new Reservation { UserId = "owner", Vehicle = v, PickupLocation = loc1, ReturnLocation = loc2, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1), Status = ReservationStatus.Booked, PriceCalculated = 1, CreatedAt = DateTime.UtcNow };
            db.Reservations.Add(r);
            await db.SaveChangesAsync();

            var sut = new ReservationService(db);
            var res = await sut.ConfirmAsync(r.Id, userId: null, isAdmin: true);

            Assert.That(res.Succeeded, Is.True);
            Assert.That((await db.Reservations.FindAsync(r.Id))!.Status, Is.EqualTo(ReservationStatus.Confirmed));
        }
    }
}
