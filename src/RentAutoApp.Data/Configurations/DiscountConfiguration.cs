using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Code).IsRequired().HasMaxLength(30);
        builder.Property(d => d.Description).HasMaxLength(200);
        builder.HasOne(d => d.User)
               .WithMany()
               .HasForeignKey(d => d.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Reservation)
               .WithMany()
               .HasForeignKey(d => d.ReservationId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
