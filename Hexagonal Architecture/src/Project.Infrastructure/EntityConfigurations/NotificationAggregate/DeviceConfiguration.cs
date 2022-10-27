using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.Infrastructure.EntityConfigurations.NotificationAggregate
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("NotificationDevice");

            builder.Property<long>("AutoIncrementPK")
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.HasKey("AutoIncrementPK");

            builder.Property(e => e.Id).IsRequired();

            builder.Property(e => e.ReadTime);

            builder.HasIndex("NotificationId", "Id").IsUnique();
        }
    }
}