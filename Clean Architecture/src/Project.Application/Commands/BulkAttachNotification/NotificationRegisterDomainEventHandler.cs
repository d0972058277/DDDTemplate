using Architecture;
using Project.Domain.Events;

namespace Project.Application.Commands.BulkAttachNotification
{
    public class NotificationRegisterDomainEventHandler : IDomainEventHandler<NotificationRegisteredDomainEvent>
    {
        private readonly IEventMediator _eventMediator;

        public NotificationRegisterDomainEventHandler(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        public async Task Handle(NotificationRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            var command = new BulkAttachNotificationCommand(notification.DeviceIds, notification.NotificationId);
            await _eventMediator.PublishCommandAsync(command, cancellationToken);
        }
    }
}