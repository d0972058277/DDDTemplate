using Architecture;

namespace Project.Domain.Events
{
    public class NotificationBulkAttachedDomainEvent : IDomainEvent
    {
        public NotificationBulkAttachedDomainEvent(IEnumerable<NotificationAttachedDomainEvent> notificationAttachedDomainEvents)
        {
            NotificationAttachedDomainEvents = notificationAttachedDomainEvents;
        }

        public IEnumerable<NotificationAttachedDomainEvent> NotificationAttachedDomainEvents { get; }
    }
}