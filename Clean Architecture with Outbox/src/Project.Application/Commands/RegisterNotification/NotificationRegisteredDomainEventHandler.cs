using Architecture;
using Project.Application.IntegrationEvents;
using Project.Domain.Events;

namespace Project.Application.Commands.RegisterNotification
{
    public class NotificationRegisteredDomainEventHandler : IDomainEventHandler<NotificationRegisteredDomainEvent>
    {
        private readonly IEventMediator _eventMediator;

        public NotificationRegisteredDomainEventHandler(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        public async Task Handle(NotificationRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new NotificationRegisteredIntegrationEvent(notification.NotificationId, notification.DeviceIds);
            await _eventMediator.PublishIntegrationEventAsync(integrationEvent, cancellationToken);
        }
    }
}