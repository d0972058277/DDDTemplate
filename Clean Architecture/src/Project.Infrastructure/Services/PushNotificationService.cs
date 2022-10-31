using Project.Application.Services;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.Infrastructure.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        public Task ExecuteAsync(Notification notification, CancellationToken cancellationToken)
        {
            // TODO: External System FCM/APNs
            return Task.CompletedTask;
        }
    }
}