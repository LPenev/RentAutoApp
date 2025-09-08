using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.DataProtection;
using RentAutoApp.Data.Models;
using RentAutoApp.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using RentAutoApp.Services.Core;
using RentAutoApp.Services.Core.Admin;
using RentAutoApp.Services.Core.Admin.Contracts;
using RentAutoApp.Services.Core.Contracts;
using RentAutoApp.Web.Data;
using RentAutoApp.Web.Features.UserPanel;
using RentAutoApp.Web.Infrastructure;
using RentAutoApp.Web.Infrastructure.Contracts;
using RentAutoApp.Web.Infrastructure.Email;


var builder = WebApplication.CreateBuilder(args);

// add clear default logs rules and add logs into console
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<RentAutoAppDbContext>(options =>
    options.UseSqlServer(connectionString));

var emailSender = builder.Configuration.GetValue<bool>("EmailSender");

// disable or enable EmailSender into appsettings.json
if (emailSender)
{
    // get email settings from appsettings.json
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
}
else
{
    // NoEmail
    builder.Services.AddSingleton<IEmailSender, RentAutoApp.Web.Infrastructure.Email.NoOpEmailSender>();
}

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
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.SlidingExpiration = true;
});

builder.Services.AddRazorPages();

builder.Services.AddRouting(o => o.LowercaseUrls = true);

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

// Custom services
builder.Services.AddScoped<ICarSearchService, CarSearchService>();
builder.Services.AddScoped<IFeaturedCarsService, FeaturedCarsService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddScoped<IErrorPageService, ErrorPageService>();

builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserPanelService, UserPanelService>();

builder.Services.AddScoped<ISettingsService, SettingsService>();

// Admin services
builder.Services.AddScoped<IAdminVehicleService, AdminVehicleService>();
builder.Services.AddScoped<IAdminReservationService, AdminReservationService>();

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddControllersWithViews(options =>
{
    // add a global filter for CSRF
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

if (builder.Environment.IsProduction())
{
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
        .SetApplicationName("RentAutoApp");
} else if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    
}

var seedDemoData = builder.Configuration.GetValue<bool>("SeedDemoData");

// DbSeeder check demo data and seed demo data when needed.
// disable when you not need demo data into appsettings.json

if (seedDemoData)
{
    builder.Services.AddScoped<DbSeeder>();
}

var app = builder.Build();

if (app.Environment.IsProduction())
{
    var fwd = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        // Attention
        // you need the reverse proxy and the web server in the same network when ForwardLimit is 1
        ForwardLimit = 1

        //fwd.KnownProxies.Add(IPAddress.Parse("172.18.0.10"));
    };

    
    fwd.KnownNetworks.Clear();
    fwd.KnownProxies.Clear();

    app.UseForwardedHeaders(fwd);

}

// Global error and HSTS only when not in Development use
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error/500");

    // Hsts is only disabled when the web server is behind a reverse proxy.
    // disabled for now, i've reverse proxy
    //app.UseHsts();
}

app.UseHttpsRedirection();


// DbSeeder check demo data into Db and seed demo data when needed.
// disable when you not need demo data into appsettings.json
if (seedDemoData)
{
    using var scope = app.Services.CreateScope();
   
        var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
        await seeder.SeedAsync();
    
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Custom pages for status codes (404, 403, 500, ...)
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Test Email Service nur in Development and EmailSender is enabled.
if (emailSender && app.Environment.IsDevelopment())
{
    app.MapGet("/dev/test-email", async (
       IEmailSender sender,
       ISettingsService settings) =>
    {
        var to = await settings.GetAsync("Contact.RecipientEmail") ?? "email@example.com";
        await sender.SendEmailAsync(to, "Test email", "<b>It works!</b>");
        return Results.Ok($"Sent to {to}");
    });
}

app.Run();
