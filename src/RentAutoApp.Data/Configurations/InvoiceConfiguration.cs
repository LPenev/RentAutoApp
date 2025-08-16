using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.InvoicePdfUrl)
                .IsRequired();

            builder.Property(i => i.IssuedAt)
                .IsRequired();

            builder.HasOne(i => i.Reservation)
                .WithOne()
                .HasForeignKey<Invoice>(i => i.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(i => !i.Reservation.Vehicle.IsArchived);
        }
    }
}
