using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class DamageReportConfiguration : IEntityTypeConfiguration<DamageReport>
{
    public void Configure(EntityTypeBuilder<DamageReport> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Description).HasMaxLength(500);
       
        builder.HasOne(d => d.Reservation)
               .WithMany()
               .HasForeignKey(d => d.ReservationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(d => new { d.ReservationId, d.UserId });

        builder.HasQueryFilter(d => !d.Reservation.Vehicle.IsArchived);
    }
}
