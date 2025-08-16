using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations
{
    public class VehicleImageConfiguration : IEntityTypeConfiguration<VehicleImage>
    {
        public void Configure(EntityTypeBuilder<VehicleImage> builder)
        {
            builder.HasKey(vi => vi.Id);

            builder.Property(vi => vi.ImageUrl)
                .IsRequired();

            builder.Property(vi => vi.UploadedAt)
                .IsRequired();

            builder.HasOne(vi => vi.Vehicle)
                .WithMany(v => v.Images)
                .HasForeignKey(vi => vi.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(vi => !vi.Vehicle.IsArchived);
        }
    }
}
