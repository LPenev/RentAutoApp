using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ServiceType).IsRequired().HasMaxLength(50);
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.HasOne(s => s.Vehicle)
               .WithMany()
               .HasForeignKey(s => s.VehicleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
