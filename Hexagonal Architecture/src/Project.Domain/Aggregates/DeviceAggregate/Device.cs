using Architecture;
using KnstArchitecture.SequentialGuid;
using Project.Domain.Events;

namespace Project.Domain.Aggregates.DeviceAggregate
{
    public class Device : Aggregate<Guid>
    {
        private Device() { }

        private Device(Guid id, Token token) : base(id)
        {
            Token = token;
        }

        public Token Token { get; } = default!;

        private List<Notification> _notifications = new List<Notification>();
        public IReadOnlyList<Notification> Notifications => _notifications;

        public static Device Register(Token token)
        {
            var id = SequentialGuid.NewGuid();
            var device = new Device(id, token);
            device.AddDomainEvent(new DeviceRegisteredDomainEvent(device.Id));
            return device;
        }

        public void Attach(Notification notification)
        {
            var notificationIds = _notifications.Select(n => n.Id).ToHashSet();

            if (notificationIds.Contains(notification.Id))
                return;

            _notifications.Add(notification);
            AddDomainEvent(new NotificationAttachedDomainEvent(Id, notification.Id));
        }

        public void Read(Guid notificationId)
        {
            var notifications = _notifications.ToDictionary(n => n.Id, n => n);

            if (notifications.TryGetValue(notificationId, out var notification))
            {
                notification.Read();
                AddDomainEvent(new NotificationReadDomainEvent(Id, notificationId));
            }
        }
    }
}