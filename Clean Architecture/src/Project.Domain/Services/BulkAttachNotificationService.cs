using Project.Domain.Aggregates.DeviceAggregate;
using Project.Domain.Events;

namespace Project.Domain.Services
{
    public static class BulkAttachNotificationService
    {
        public static NotificationBulkAttachedDomainEvent Execute(IEnumerable<Device> devices, Guid notificationId)
        {
            var notificationAttachedDomainEvents = new List<NotificationAttachedDomainEvent>();

            foreach (var device in devices)
            {
                var notification = Notification.Create(notificationId);
                device.Attach(notification);

                var domainEvents = device.GetAndClearDomainEvents();
                var notificationAttachedDomainEvent = domainEvents.Where(d => d is NotificationAttachedDomainEvent).SingleOrDefault();
                if (notificationAttachedDomainEvent is not null)
                {
                    notificationAttachedDomainEvents.Add((NotificationAttachedDomainEvent)notificationAttachedDomainEvent);
                }
            }

            return new NotificationBulkAttachedDomainEvent(notificationAttachedDomainEvents);
        }
    }
}