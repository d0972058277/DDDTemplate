using Architecture;
using MediatR;
using Project.Application.Repositories;
using Project.Domain.Services;

namespace Project.Application.Commands.BulkAttachNotification
{
    public class BulkAttachNotificationCommandHandler : ICommandHandler<BulkAttachNotificationCommand>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IEventMediator _eventMediator;

        public BulkAttachNotificationCommandHandler(IDeviceRepository deviceRepository, IEventMediator eventMediator)
        {
            _deviceRepository = deviceRepository;
            _eventMediator = eventMediator;
        }

        public async Task<Unit> Handle(BulkAttachNotificationCommand request, CancellationToken cancellationToken)
        {
            var devices = await _deviceRepository.FindsAsync(request.DeviceIds, cancellationToken);
            var domainEvent = BulkAttachNotificationService.Execute(devices, request.NotificationId);
            await _deviceRepository.SavesAsync(devices, cancellationToken);
            await _eventMediator.PublishDomainEventAsync(domainEvent, cancellationToken);
            return Unit.Value;
        }
    }
}