using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);
        builder.HasOne(r => r.Vehicle).WithMany().HasForeignKey(r => r.VehicleId);
        builder.HasOne(r => r.PickupLocation).WithMany().HasForeignKey(r => r.PickupLocationId);
        builder.HasOne(r => r.ReturnLocation).WithMany().HasForeignKey(r => r.ReturnLocationId);
        builder.Property(r => r.PriceCalculated).HasColumnType("decimal(18,2)");
        builder.HasOne(r => r.ReturnLocation)
            .WithMany()
            .HasForeignKey(r => r.ReturnLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.PickupLocation)
            .WithMany()
            .HasForeignKey(r => r.PickupLocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
