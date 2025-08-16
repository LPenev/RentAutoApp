using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;
public class RepairHistoryConfiguration : IEntityTypeConfiguration<RepairHistory>
{
    public void Configure(EntityTypeBuilder<RepairHistory> builder)
    {
        builder.HasKey(r => r.Id);

        builder
            .Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder
            .Property(r => r.RepairShop)
            .HasMaxLength(255);

        builder
            .Property(r => r.Notes)
            .HasMaxLength(1000);

        builder
            .Property(r => r.Cost)
            .HasColumnType("decimal(18,2)");

        builder
            .HasOne(r => r.Vehicle)
            .WithMany(v => v.RepairHistories)
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(r => !r.Vehicle.IsArchived);
    }
}
