using Architecture;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.Application.Services
{
    public interface IPushNotificationService : IApplicationService
    {
        Task ExecuteAsync(Notification notification, CancellationToken cancellationToken);
    }
}