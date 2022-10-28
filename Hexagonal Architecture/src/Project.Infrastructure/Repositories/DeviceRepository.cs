using Microsoft.EntityFrameworkCore;
using Project.Application.Repositories;
using Project.Domain.Aggregates.DeviceAggregate;

namespace Project.Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ProjectDbContext _dbContext;

        public DeviceRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(Device device, CancellationToken cancellationToken)
        {
            _dbContext.Devices.Add(device);
            return Task.CompletedTask;
        }

        public async Task<Device> FindAsync(Guid deviceId, Guid notificationId, CancellationToken cancellationToken)
        {
            var device = (await _dbContext.Devices.FindAsync(deviceId))!;
            await _dbContext.Entry(device).Collection(d => d.Notifications).Query().Where(n => n.Id == notificationId).LoadAsync();
            return device;
        }

        public async Task<IReadOnlyList<Device>> FindsAsync(IEnumerable<Guid> deviceIds, CancellationToken cancellationToken)
        {
            var devices = await _dbContext.Devices.Where(d => deviceIds.Contains(d.Id)).ToListAsync();
            return devices;
        }

        public Task SaveAsync(Device device, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SavesAsync(IEnumerable<Device> devices, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}