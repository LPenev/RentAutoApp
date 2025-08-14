using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations;

public sealed class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
{
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        builder.ToTable("SiteSettings");

        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.Key)
            .IsUnique();

        builder.Property(s => s.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Value)
            .IsRequired()
            .HasMaxLength(4000);

        builder.HasData(new SiteSetting
         {
             Id = 1,
             Key = "Contact.RecipientEmail",
             Value = "lppenev@abv.bg",
             UpdatedOnUtc = DateTime.UtcNow
         });
    }
}
