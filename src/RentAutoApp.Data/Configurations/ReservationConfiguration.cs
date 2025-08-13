using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.UserId).IsRequired();
        builder.Property(r => r.VehicleId).IsRequired();
        builder.Property(r => r.PickupLocationId).IsRequired();
        builder.Property(r => r.ReturnLocationId).IsRequired();
        builder.Property(r => r.PriceCalculated).HasColumnType("decimal(18,2)");

        builder.HasOne(r => r.Vehicle)
            .WithMany(v => v.Reservations)
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.PickupLocation)
            .WithMany()
            .HasForeignKey(r => r.PickupLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ReturnLocation)
            .WithMany()
            .HasForeignKey(r => r.ReturnLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.User)
            .WithMany() //.WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.VehicleId);
        builder.HasIndex(r => r.PickupLocationId);
        builder.HasIndex(r => r.ReturnLocationId);
        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => new { r.StartDate, r.EndDate });
    }
}
