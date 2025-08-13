using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentAutoApp.Data.Models;
using RentAutoApp.GCommon.Enums;
using RentAutoApp.Web.Data;

namespace RentAutoApp.Data.Seeding
{
    public class DbSeeder
    {
        private readonly RentAutoAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbSeeder(
            RentAutoAppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // 0) add migration, when seeding
            await _context.Database.MigrateAsync();

            // 1) Roles
            var roles = new[] { "Administrator", "Staff", "Client" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var res = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (!res.Succeeded)
                        throw new InvalidOperationException(string.Join("; ", res.Errors.Select(e => e.Description)));
                }
            }

            // 2) Users (+ denomalisation for display)
            var users = new (string Email, string Password, string Role)[]
            {
                ("admin@rentauto.local",  "Admin123!",  "Administrator"),
                ("staff@rentauto.local",  "Staff123!",  "Staff"),
                ("client@rentauto.local", "Client123!", "Client")
            };

            foreach (var (email, password, role) in users)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        FirstName = email.Split('@')[0],
                        LastName = "User",
                        Role = role // denomalisation for display
                    };

                    var create = await _userManager.CreateAsync(user, password);
                    if (!create.Succeeded)
                        throw new InvalidOperationException(string.Join("; ", create.Errors.Select(e => e.Description)));

                    await _userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    if (!await _userManager.IsInRoleAsync(user, role))
                        await _userManager.AddToRoleAsync(user, role);

                    if (user.Role != role)
                    {
                        user.Role = role;
                        await _userManager.UpdateAsync(user);
                    }
                }
            }

            // 3) Countries
            var bg = await _context.Countries.FirstOrDefaultAsync(c => c.Name == "Bulgaria");
            if (bg is null)
            {
                bg = new Country { Name = "Bulgaria" };
                _context.Countries.Add(bg);
                await _context.SaveChangesAsync();
            }

            // 4) Towns
            var cityNames = new[] { "София", "Варна", "Бургас", "Пловдив" };
            var cities = new Dictionary<string, City>();
            foreach (var name in cityNames)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == name && c.CountryId == bg.Id);
                if (city is null)
                {
                    city = new City { Name = name, CountryId = bg.Id };
                    _context.Cities.Add(city);
                }
                cities[name] = city;
            }
            await _context.SaveChangesAsync();

            // 5) Streets
            var desiredStreets = new (string Name, string Number, string PostalCode, string City)[]
            {
                ("бул.Витоша",       "69", "1000", "София"),
                ("ул.Граф Игнатиев",   "25", "1142", "София"),
                ("ул.Цар Освободител",   "15", "1504", "София"),
                ("бул.България",  "5",  "1408", "София"),
                ("ул.Цар Освободител",   "81", "9000", "Варна"),
                ("бул.България",  "15", "8000", "Бургас"),
            };

            foreach (var s in desiredStreets)
            {
                var city = cities[s.City];
                var exists = await _context.Streets.AnyAsync(x =>
                    x.Name == s.Name && x.Number == s.Number && x.CityId == city.Id);
                if (!exists)
                {
                    _context.Streets.Add(new Street
                    {
                        Name = s.Name,
                        Number = s.Number,
                        PostalCode = s.PostalCode,
                        CityId = city.Id
                    });
                }
            }
            await _context.SaveChangesAsync();

            // Намираме улици за по-нататъшна употреба
            var sofia = cities["София"];
            var varna = cities["Варна"];
            var s_vitosha69 = await _context.Streets.SingleAsync(s =>
                s.Name == "бул.Витоша" && s.Number == "69" && s.CityId == sofia.Id);
            var s_graf25 = await _context.Streets.SingleAsync(s =>
                s.Name == "ул.Граф Игнатиев" && s.Number == "25" && s.CityId == sofia.Id);
            var s_tsar81_varna = await _context.Streets.SingleAsync(s =>
                s.Name == "ул.Цар Освободител" && s.Number == "81" && s.CityId == varna.Id);

            // 6) Локации (Country + City + Street задължителни)
            var wantedLocations = new (City City, Street Street)[]
            {
                (sofia, s_vitosha69),
                (sofia, s_graf25),
                (varna, s_tsar81_varna),
            };

            foreach (var w in wantedLocations)
            {
                bool exists = await _context.Locations.AnyAsync(l =>
                    l.CountryId == bg.Id && l.CityId == w.City.Id && l.StreetId == w.Street.Id);

                if (!exists)
                {
                    _context.Locations.Add(new Location
                    {
                        CountryId = bg.Id,
                        CityId = w.City.Id,
                        StreetId = w.Street.Id
                    });
                }
            }
            await _context.SaveChangesAsync();

            // 7) Категории и подкатегории
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Cars");
            if (category is null)
            {
                category = new Category { Name = "Cars" };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            var subcatNames = new[] { "Economy", "Luxury" };
            var subcats = new Dictionary<string, SubCategory>();
            foreach (var scName in subcatNames)
            {
                var sc = await _context.SubCategories.FirstOrDefaultAsync(s =>
                    s.Name == scName && s.CategoryId == category.Id);
                if (sc is null)
                {
                    sc = new SubCategory { Name = scName, CategoryId = category.Id };
                    _context.SubCategories.Add(sc);
                }
                subcats[scName] = sc;
            }
            await _context.SaveChangesAsync();

            var econ = subcats["Economy"];
            var lux = subcats["Luxury"];

            // 8) Превозни средства (уникални VIN-ове)
            var vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    Brand = "Mercedes",
                    Model = "C43 AMG",
                    RegistrationNumber = "SMB1223",
                    FirstRegistrationDate = new DateTime(2023,5,22),
                    Vin = "WDB11111111111111",
                    FuelType = FuelType.Petrol,
                    Transmission = TransmissionType.Automatic,
                    PowerHp = 150,
                    Seats = 5,
                    Doors = 4,
                    TrunkCapacity = 200,
                    Mileage = 1000,
                    PricePerDay = 155,
                    PricePerHour = 50,
                    IsAvailable = true,
                    SubCategoryId = econ.Id,
                    LocationId = 2,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { ImageUrl = "/images/vehicles/1/image1.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/1/image2.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/1/image3.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/1/image4.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/1/image5.png" }
                    },
                },
                new Vehicle
                {
                    Brand = "Audi",
                    Model = "A4",
                    RegistrationNumber = "INA4251",
                    FirstRegistrationDate = new DateTime(2023,5,22),
                    Vin = "WAU22222222222222",
                    FuelType = FuelType.Diesel,
                    Transmission = TransmissionType.Automatic,
                    PowerHp = 150,
                    Seats = 5,
                    Doors = 4,
                    TrunkCapacity = 500,
                    Mileage = 11000,
                    PricePerDay = 55,
                    PricePerHour = 10,
                    IsAvailable = true,
                    SubCategoryId = econ.Id,
                    LocationId = 1,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { ImageUrl = "/images/vehicles/2/image1.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/2/image2.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/2/image3.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/2/image4.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/2/image5.png" }
                    },
                },
                new Vehicle
                {
                    Brand = "Mercedes",
                    Model = "S350 CDI",
                    RegistrationNumber = "SMB1003",
                    FirstRegistrationDate = new DateTime(2023,5,22),
                    Vin = "WDB33333333333333",
                    FuelType = FuelType.Petrol,
                    Transmission = TransmissionType.Automatic,
                    PowerHp = 286,
                    Seats = 5,
                    Doors = 4,
                    TrunkCapacity = 200,
                    Mileage = 9000,
                    PricePerDay = 155,
                    PricePerHour = 50,
                    IsAvailable = true,
                    SubCategoryId = lux.Id,
                    LocationId = 2,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { ImageUrl = "/images/vehicles/3/image1.jpg" },
                        new VehicleImage { ImageUrl = "/images/vehicles/3/image2.jpg" },
                        new VehicleImage { ImageUrl = "/images/vehicles/3/image3.jpg" },
                        new VehicleImage { ImageUrl = "/images/vehicles/3/image4.jpg" },
                        new VehicleImage { ImageUrl = "/images/vehicles/3/image5.jpg" }
                    },
                },
                new Vehicle
                {
                    Brand = "Audi",
                    Model = "A8",
                    RegistrationNumber = "CA1234BA",
                    FirstRegistrationDate = new DateTime(2023,5,22),
                    Vin = "WAU44444444444444",
                    FuelType = FuelType.Petrol,
                    Transmission = TransmissionType.Automatic,
                    PowerHp = 450,
                    Seats = 5,
                    Doors = 4,
                    TrunkCapacity = 300,
                    Mileage = 4000,
                    PricePerDay = 155,
                    PricePerHour = 55,
                    IsAvailable = true,
                    SubCategoryId = lux.Id,
                    LocationId = 3,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { ImageUrl = "/images/vehicles/4/image1.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/4/image2.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/4/image3.png" },
                    },
                },
                new Vehicle
                {
                    Brand = "Mercedes",
                    Model = "EQS",
                    RegistrationNumber = "SEQ133E",
                    FirstRegistrationDate = new DateTime(2024,2,12),
                    Vin = "W1K55555555555555",
                    FuelType = FuelType.Electric,
                    Transmission = TransmissionType.Automatic,
                    PowerHp = 440,
                    Seats = 5,
                    Doors = 4,
                    TrunkCapacity = 500,
                    Mileage = 2000,
                    PricePerDay = 125,
                    PricePerHour = 40,
                    IsAvailable = true,
                    SubCategoryId = lux.Id,
                    LocationId = 3,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { ImageUrl = "/images/vehicles/5/image1.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/5/image2.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/5/image3.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/5/image4.png" },
                        new VehicleImage { ImageUrl = "/images/vehicles/5/image5.png" }
                    },
                },
            };

            foreach (var v in vehicles)
            {
                var exists = await _context.Vehicles.AnyAsync(x => x.Vin == v.Vin);
                if (!exists) _context.Vehicles.Add(v);
            }
            await _context.SaveChangesAsync();

            // 9) Примерна резервация за клиента
            var clientUser = await _userManager.FindByEmailAsync("client@rentauto.local");
            if (clientUser is not null)
            {
                var v1 = await _context.Vehicles.FirstOrDefaultAsync(v => v.Vin == "WDB11111111111111");
                if (v1 is not null)
                {
                    var pickupStreet = s_graf25;
                    var pickupLocation = await _context.Locations.FirstOrDefaultAsync(l =>
                        l.CountryId == bg.Id && l.CityId == sofia.Id && l.StreetId == pickupStreet.Id)
                        ?? await _context.Locations.FirstAsync(); // fallback

                    var reservationExists = await _context.Reservations.AnyAsync(r =>
                        r.UserId == clientUser.Id && r.VehicleId == v1.Id);

                    if (!reservationExists)
                    {
                        var reservation = new Reservation
                        {
                            VehicleId = v1.Id,
                            UserId = clientUser.Id,
                            PickupLocationId = pickupLocation.Id,
                            ReturnLocationId = pickupLocation.Id,
                            StartDate = DateTime.Today,
                            EndDate = DateTime.Today.AddDays(3),
                            Status = ReservationStatus.Confirmed
                        };

                        _context.Reservations.Add(reservation);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
