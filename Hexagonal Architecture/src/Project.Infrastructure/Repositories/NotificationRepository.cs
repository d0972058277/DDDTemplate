using Microsoft.EntityFrameworkCore;
using Project.Application.Repositories;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ProjectDbContext _dbContext;

        public NotificationRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(Notification notification, CancellationToken cancellationToken)
        {
            _dbContext.Notifications.Add(notification);
            return Task.CompletedTask;
        }

        public async Task<Notification> FindAsync(Guid notificationId, CancellationToken cancellationToken)
        {
            var notification = (await _dbContext.Notifications.FindAsync(notificationId))!;
            return notification;
        }

        public async Task<Notification> FindAsync(Guid notificationId, Guid deviceId, CancellationToken cancellationToken)
        {
            var notification = (await _dbContext.Notifications.FindAsync(notificationId))!;
            await _dbContext.Entry(notification).Collection(n => n.Devices).Query().Where(d => d.Id == deviceId).LoadAsync();
            return notification;
        }

        public Task SaveAsync(Notification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}