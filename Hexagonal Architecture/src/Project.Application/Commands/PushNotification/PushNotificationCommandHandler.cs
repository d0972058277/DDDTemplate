using Architecture;
using MediatR;
using Project.Application.Repositories;
using Project.Application.Services;

namespace Project.Application.Commands.PushNotification
{
    public class PushNotificationCommandHandler : ICommandHandler<PushNotificationCommand>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventMediator _eventMediator;
        private readonly IPushNotificationService _pushNotificationService;

        public PushNotificationCommandHandler(INotificationRepository notificationRepository, IEventMediator eventMediator, IPushNotificationService pushNotificationService)
        {
            _notificationRepository = notificationRepository;
            _eventMediator = eventMediator;
            _pushNotificationService = pushNotificationService;
        }

        public async Task<Unit> Handle(PushNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.FindAsync(request.NotificationId, cancellationToken);
            await _pushNotificationService.ExecuteAsync(notification, cancellationToken);
            notification.Push();
            await _notificationRepository.SaveAsync(notification, cancellationToken);
            await _eventMediator.PublishDomainEventsAsync(notification.GetAndClearDomainEvents(), cancellationToken);
            return Unit.Value;
        }
    }
}