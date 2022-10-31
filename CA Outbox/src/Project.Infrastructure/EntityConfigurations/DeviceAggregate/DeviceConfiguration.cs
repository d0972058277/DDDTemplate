using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Aggregates.DeviceAggregate;

namespace Project.Infrastructure.EntityConfigurations.DeviceAggregate
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("Device");

            builder.Ignore(device => device.DomainEvents);

            builder.HasKey(device => device.Id);

            builder.OwnsOne(device => device.Token, token =>
            {
                token.Property(e => e.Value);
                token.WithOwner();
            }).Navigation(device => device.Token).IsRequired();

            builder.HasMany(device => device.Notifications)
                .WithOne()
                .HasForeignKey("DeviceId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            builder.Navigation(b => b.Notifications).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}