using Architecture;

namespace Project.Domain.Events
{
    public class DeviceRegisteredDomainEvent : IDomainEvent
    {
        public DeviceRegisteredDomainEvent(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; }
    }
}