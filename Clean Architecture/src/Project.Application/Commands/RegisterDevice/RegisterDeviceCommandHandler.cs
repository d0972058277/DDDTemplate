using Architecture;
using Project.Application.Repositories;
using Project.Domain.Aggregates.DeviceAggregate;

namespace Project.Application.Commands.RegisterDevice
{
    public class RegisterDeviceCommandHandler : ICommandHandler<RegisterDeviceCommand, Guid>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IEventMediator _eventMediator;

        public RegisterDeviceCommandHandler(IDeviceRepository deviceRepository, IEventMediator eventMediator)
        {
            _deviceRepository = deviceRepository;
            _eventMediator = eventMediator;
        }

        public async Task<Guid> Handle(RegisterDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = Device.Register(request.Token);
            await _deviceRepository.AddAsync(device, cancellationToken);
            await _eventMediator.PublishDomainEventsAsync(device.GetAndClearDomainEvents(), cancellationToken);
            return device.Id;
        }
    }
}