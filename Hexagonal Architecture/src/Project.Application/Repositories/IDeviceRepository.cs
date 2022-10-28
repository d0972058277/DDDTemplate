using Architecture;
using Project.Domain.Aggregates.DeviceAggregate;

namespace Project.Application.Repositories
{
    public interface IDeviceRepository : IRepository
    {
        Task AddAsync(Device device, CancellationToken cancellationToken);
        Task<Device> FindAsync(Guid deviceId, Guid notificationId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Device>> FindsAsync(IEnumerable<Guid> deviceIds, CancellationToken cancellationToken);
        Task SaveAsync(Device device, CancellationToken cancellationToken);
        Task SavesAsync(IEnumerable<Device> devices, CancellationToken cancellationToken);
    }
}