using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ContractPdfUrl)
                .IsRequired();

            builder.HasOne(c => c.Reservation)
                .WithOne()
                .HasForeignKey<Contract>(c => c.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(c => !c.Reservation.Vehicle.IsArchived);
        }
    }
}
