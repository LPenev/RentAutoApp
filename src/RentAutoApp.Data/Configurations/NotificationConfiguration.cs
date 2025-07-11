using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentAutoApp.Data.Models;

namespace RentAutoApp.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Type)
                .IsRequired();

            builder.Property(n => n.Message)
                .IsRequired();

            builder.Property(n => n.ScheduledAt)
                .IsRequired();

            builder.Property(n => n.IsRead)
                .IsRequired();

            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Vehicle)
                .WithMany()
                .HasForeignKey(n => n.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Reservation)
                .WithMany()
                .HasForeignKey(n => n.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Document)
                .WithMany()
                .HasForeignKey(n => n.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Service)
                .WithMany()
                .HasForeignKey(n => n.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
