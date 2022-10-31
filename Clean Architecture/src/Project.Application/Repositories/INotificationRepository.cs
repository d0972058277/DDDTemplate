using Architecture;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.Application.Repositories
{
    public interface INotificationRepository : IRepository
    {
        Task AddAsync(Notification notification, CancellationToken cancellationToken);
        Task<Notification> FindAsync(Guid notificationId, CancellationToken cancellationToken);
        Task<Notification> FindAsync(Guid notificationId, Guid deviceId, CancellationToken cancellationToken);
        Task SaveAsync(Notification notification, CancellationToken cancellationToken);
    }
}