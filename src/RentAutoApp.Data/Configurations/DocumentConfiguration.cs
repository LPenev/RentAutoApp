using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.DocType).IsRequired().HasMaxLength(50);
        builder.HasOne(d => d.Vehicle)
               .WithMany()
               .HasForeignKey(d => d.VehicleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
