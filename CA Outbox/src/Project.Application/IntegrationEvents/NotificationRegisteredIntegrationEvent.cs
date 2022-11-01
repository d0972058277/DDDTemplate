using Architecture;

namespace Project.Application.IntegrationEvents
{
    public class NotificationRegisteredIntegrationEvent : IntegrationEvent
    {
        public NotificationRegisteredIntegrationEvent(Guid notificationId, IEnumerable<Guid> deviceIds)
        {
            NotificationId = notificationId;
            DeviceIds = deviceIds;
        }

        public Guid NotificationId { get; }
        public IEnumerable<Guid> DeviceIds { get; }
    }
}