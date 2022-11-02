using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.Infrastructure.EntityConfigurations.NotificationAggregate
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            builder.Ignore(notification => notification.DomainEvents);

            builder.HasKey(notification => notification.Id);

            builder.OwnsOne(notification => notification.Message, message =>
            {
                message.Property(e => e.Title);
                message.Property(e => e.Body);
                message.WithOwner();
            }).Navigation(notification => notification.Message).IsRequired();

            builder.OwnsOne(notification => notification.Schedule, schedule =>
            {
                schedule.Property(e => e.Value);
                schedule.WithOwner();
            }).Navigation(notification => notification.Schedule).IsRequired();

            builder.HasMany(notification => notification.Devices)
                .WithOne()
                .HasForeignKey("NotificationId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            builder.Navigation(b => b.Devices).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(notification => notification.PushedTime);
        }
    }
}