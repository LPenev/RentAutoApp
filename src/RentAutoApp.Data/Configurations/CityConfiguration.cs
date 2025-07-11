using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasOne(c => c.Country)
               .WithMany(cn => cn.Cities)
               .HasForeignKey(c => c.CountryId);

        builder.HasMany(c => c.Streets)
               .WithOne(s => s.City)
               .HasForeignKey(s => s.CityId);
    }
}
