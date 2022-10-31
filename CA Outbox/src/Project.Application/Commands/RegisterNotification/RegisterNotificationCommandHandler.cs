using Architecture;
using Project.Application.Repositories;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.Application.Commands.RegisterNotification
{
    public class RegisterNotificationCommandHandler : ICommandHandler<RegisterNotificationCommand, Guid>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMediator _eventMediator;

        public RegisterNotificationCommandHandler(INotificationRepository notificationRepository, IEventMediator eventMediator)
        {
            _notificationRepository = notificationRepository;
            _eventMediator = eventMediator;
        }

        public async Task<Guid> Handle(RegisterNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = Notification.Register(request.Message, request.Schedule, request.DeviceIds.Select(id => Device.Create(id)).ToHashSet());
            await _notificationRepository.AddAsync(notification, cancellationToken);
            await _eventMediator.PublishDomainEventsAsync(notification.GetAndClearDomainEvents(), cancellationToken);
            return notification.Id;
        }
    }
}