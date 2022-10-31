using Architecture;

namespace Project.Domain.Events
{
    public class NotificationRegisteredDomainEvent : IDomainEvent
    {
        public NotificationRegisteredDomainEvent(Guid notificationId, IEnumerable<Guid> deviceIds)
        {
            NotificationId = notificationId;
            DeviceIds = deviceIds;
        }

        public Guid NotificationId { get; }
        public IEnumerable<Guid> DeviceIds { get; }
    }
}