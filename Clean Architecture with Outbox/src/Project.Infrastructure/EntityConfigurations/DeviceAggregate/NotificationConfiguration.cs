using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Aggregates.DeviceAggregate;

namespace Project.Infrastructure.EntityConfigurations.DeviceAggregate
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("DeviceNotification");

            builder.Property<long>("AutoIncrementPK")
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.HasKey("AutoIncrementPK");

            builder.Property(e => e.Id).IsRequired();

            builder.Property(e => e.ReadTime);

            builder.HasIndex("DeviceId", "Id").IsUnique();
        }
    }
}