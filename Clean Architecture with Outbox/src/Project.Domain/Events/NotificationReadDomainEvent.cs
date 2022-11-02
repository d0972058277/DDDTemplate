using Architecture;

namespace Project.Domain.Events
{
    public class NotificationReadDomainEvent : IDomainEvent
    {
        public NotificationReadDomainEvent(Guid deviceId, Guid notificationId)
        {
            DeviceId = deviceId;
            NotificationId = notificationId;
        }

        public Guid DeviceId { get; }
        public Guid NotificationId { get; }
    }
}