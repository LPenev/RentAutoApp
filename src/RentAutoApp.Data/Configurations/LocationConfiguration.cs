using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);

        builder.HasOne(l => l.Country)
               .WithMany()
               .HasForeignKey(l => l.CountryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.City)
               .WithMany()
               .HasForeignKey(l => l.CityId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Street)
               .WithMany()
               .HasForeignKey(l => l.StreetId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
