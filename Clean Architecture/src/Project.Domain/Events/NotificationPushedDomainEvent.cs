using Architecture;

namespace Project.Domain.Events
{
    public class NotificationPushedDomainEvent : IDomainEvent
    {
        public NotificationPushedDomainEvent(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; }
    }
}