using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RentAutoApp.Data.Models;
using RentAutoApp.Data.Seeding;
using RentAutoApp.Services.Core;
using RentAutoApp.Services.Core.Admin;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Services.Core.Contracts.Admin;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.Features.UserPanel;
using RentAutoApp.Web.Infrastructure;
using RentAutoApp.Web.Infrastructure.Contracts;
using RentAutoApp.Web.Infrastructure.Email;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<RentAutoAppDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// E-Mail
//builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email:Smtp"));
//builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

//builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// NoEmail
builder.Services.AddSingleton<IEmailSender, RentAutoApp.Web.Infrastructure.Email.NoOpEmailSender>();


builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 3;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<RentAutoAppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IFeaturedCarsService, FeaturedCarsService>();
builder.Services.AddScoped<ICarSearchService, CarSearchService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserPanelService, UserPanelService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Admin services
builder.Services.AddScoped<IAdminVehicleService, AdminVehicleService>();
builder.Services.AddScoped<IAdminReservationService, AdminReservationService>();

// check demo data and seed demo data when needed.
builder.Services.AddScoped<DbSeeder>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});


var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Test Email Service
//if (app.Environment.IsDevelopment())
//{
//    app.MapGet("/dev/test-email", async (
//        Microsoft.AspNetCore.Identity.UI.Services.IEmailSender sender,
//        RentAutoApp.Services.Core.Contracts.ISettingsService settings) =>
//    {
//        var to = await settings.GetAsync("Contact.RecipientEmail") ?? "email@example.com";
//        await sender.SendEmailAsync(to, "Test email", "<b>It works!</b>");
//        return Results.Ok($"Sent to {to}");
//    });
//}

app.Run();
