using Architecture;

namespace Project.Domain.Events
{
    public class NotificationAttachedDomainEvent : IDomainEvent
    {
        public NotificationAttachedDomainEvent(Guid deviceId, Guid notificationId)
        {
            DeviceId = deviceId;
            NotificationId = notificationId;
        }

        public Guid DeviceId { get; }
        public Guid NotificationId { get; }
    }
}