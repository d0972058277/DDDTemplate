using Architecture;
using KnstArchitecture.SequentialGuid;
using Project.Domain.Events;

namespace Project.Domain.Aggregates.NotificationAggregate
{
    public class Notification : Aggregate<Guid>
    {
        private Notification() { }

        private Notification(Guid id, Message message, Schedule schedule, List<Device> devices) : base(id)
        {
            Message = message;
            Schedule = schedule;
            _devices = devices;
        }

        public Message Message { get; } = default!;

        public Schedule Schedule { get; } = default!;

        private List<Device> _devices = new List<Device>();
        public IReadOnlyList<Device> Devices => _devices;

        public DateTime? PushedTime { get; private set; }

        public static Notification Register(Message message, Schedule schedule, HashSet<Device> devices)
        {
            var id = SequentialGuid.NewGuid();
            var notification = new Notification(id, message, schedule, devices.ToList());
            notification.AddDomainEvent(new NotificationRegisteredDomainEvent(notification.Id, devices.Select(d => d.Id).ToList()));
            return notification;
        }

        public void Push()
        {
            PushedTime = SystemDateTime.UtcNow;
            AddDomainEvent(new NotificationPushedDomainEvent(Id));
        }

        public void Read(Guid deviceId)
        {
            var devices = _devices.ToDictionary(d => d.Id, d => d);

            if (devices.TryGetValue(deviceId, out var device))
            {
                device.Read();
                AddDomainEvent(new DeviceReadDomainEvent(Id, deviceId));
            }
        }
    }
}