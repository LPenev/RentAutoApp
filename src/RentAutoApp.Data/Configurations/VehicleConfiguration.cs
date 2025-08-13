using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Brand).IsRequired().HasMaxLength(50);
        builder.Property(v => v.Model).IsRequired().HasMaxLength(50);
        builder.Property(v => v.FuelType).IsRequired();
        builder.Property(v => v.Transmission).IsRequired();
        builder.Property(v => v.RegistrationNumber).IsRequired().HasMaxLength(20);
        builder.Property(v => v.Vin).IsRequired().HasMaxLength(50);
        builder.Property(v => v.PricePerDay).HasColumnType("decimal(18,2)");
        builder.Property(v => v.PricePerHour).HasColumnType("decimal(18,2)");
        builder.Property(v => v.Doors).IsRequired().HasDefaultValue(4);
        
        builder.HasCheckConstraint("CK_Vehicles_Doors", "[Doors] >= 2 AND [Doors] <= 6");

        builder.HasOne(v => v.SubCategory)
               .WithMany()
               .HasForeignKey(v => v.SubCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Location)
            .WithMany(l => l.Vehicles) 
            .HasForeignKey(v => v.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
