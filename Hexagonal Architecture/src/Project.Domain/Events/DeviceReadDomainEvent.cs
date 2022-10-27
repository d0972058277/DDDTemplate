using Architecture;

namespace Project.Domain.Events
{
    public class DeviceReadDomainEvent : IDomainEvent
    {
        public DeviceReadDomainEvent(Guid notificationId, Guid deviceId)
        {
            NotificationId = notificationId;
            DeviceId = deviceId;
        }

        public Guid NotificationId { get; }
        public Guid DeviceId { get; }
    }
}