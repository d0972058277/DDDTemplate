using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using KnstArchitecture.SequentialGuid;

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
            return device;
        }

        public void Attach(Notification notification)
        {
            var notificationIds = _notifications.Select(n => n.Id).ToHashSet();

            if (notificationIds.Contains(notification.Id))
                return;

            _notifications.Add(notification);
        }
    }
}