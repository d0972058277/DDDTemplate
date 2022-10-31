using Architecture;
using MediatR;
using Project.Application.Repositories;

namespace Project.Application.Commands.ReadNotification
{
    public class ReadNotificationCommandHandler : ICommandHandler<ReadNotificationCommand>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IEventMediator _eventMediator;

        public ReadNotificationCommandHandler(IDeviceRepository deviceRepository, IEventMediator eventMediator)
        {
            _deviceRepository = deviceRepository;
            _eventMediator = eventMediator;
        }

        public async Task<Unit> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
        {
            var device = await _deviceRepository.FindAsync(request.DeviceId, request.NotificationId, cancellationToken);
            device.Read(request.NotificationId);
            await _deviceRepository.SaveAsync(device, cancellationToken);
            await _eventMediator.PublishDomainEventsAsync(device.GetAndClearDomainEvents(), cancellationToken);
            return Unit.Value;
        }
    }
}