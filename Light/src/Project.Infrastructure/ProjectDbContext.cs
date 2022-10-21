using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.EntityConfigurations.DeviceAggregate;

namespace Project.Infrastructure
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Domain.Aggregates.DeviceAggregate.Device> Devices => Set<Domain.Aggregates.DeviceAggregate.Device>();
        public DbSet<Domain.Aggregates.DeviceAggregate.Notification> DeviceNotifications => Set<Domain.Aggregates.DeviceAggregate.Notification>();
        public DbSet<Domain.Aggregates.NotificationAggregate.Notification> Notifications => Set<Domain.Aggregates.NotificationAggregate.Notification>();
        public DbSet<Domain.Aggregates.NotificationAggregate.Device> NotificationDevices => Set<Domain.Aggregates.NotificationAggregate.Device>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityConfigurations.DeviceAggregate.DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.DeviceAggregate.NotificationConfiguration());

            modelBuilder.ApplyConfiguration(new EntityConfigurations.NotificationAggregate.NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.NotificationAggregate.DeviceConfiguration());
        }
    }
}