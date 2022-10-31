using Architecture;
using MediatR;
using Project.Application.Repositories;

namespace Project.Application.Commands.ReadDevice
{
    public class ReadDeviceCommandHandler : ICommandHandler<ReadDeviceCommand>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMediator _eventMediator;

        public ReadDeviceCommandHandler(INotificationRepository notificationRepository, IEventMediator eventMediator)
        {
            _notificationRepository = notificationRepository;
            _eventMediator = eventMediator;
        }

        public async Task<Unit> Handle(ReadDeviceCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.FindAsync(request.NotificationId, request.DeviceId, cancellationToken);
            notification.Read(request.DeviceId);
            await _notificationRepository.SaveAsync(notification, cancellationToken);
            await _eventMediator.PublishDomainEventsAsync(notification.GetAndClearDomainEvents(), cancellationToken);
            return Unit.Value;
        }
    }
}