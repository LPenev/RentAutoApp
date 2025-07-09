using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentAutoApp.Data.Models;
using System.Reflection;

namespace RentAutoApp.Web.Data;

public class RentAutoAppDbContext : IdentityDbContext
{
    public RentAutoAppDbContext(DbContextOptions<RentAutoAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<RepairHistory> RepairHistories { get; set; }
    public DbSet<Document> VehicleDocuments { get; set; }
    public DbSet<Location> Locations { get; set; }
}