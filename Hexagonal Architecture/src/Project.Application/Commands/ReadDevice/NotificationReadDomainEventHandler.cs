using Architecture;
using Project.Domain.Events;

namespace Project.Application.Commands.ReadDevice
{
    public class NotificationReadDomainEventHandler : IDomainEventHandler<NotificationReadDomainEvent>
    {
        private readonly IEventMediator _eventMediator;

        public NotificationReadDomainEventHandler(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        public async Task Handle(NotificationReadDomainEvent notification, CancellationToken cancellationToken)
        {
            var command = new ReadDeviceCommand(notification.NotificationId, notification.DeviceId);
            await _eventMediator.PublishCommandAsync(command, cancellationToken);
        }
    }
}