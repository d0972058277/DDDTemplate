using Architecture;
using Project.Application.IntegrationEvents;

namespace Project.Application.Commands.RegisterNotification
{
    // NOTE: 這邊的 NotificationRegisteredIntegrationEventHandler 只是模擬，理應當要放到對應 BoundedContext 的專案當中
    public class NotificationRegisteredIntegrationEventHandler : IntegrationEventHandler<NotificationRegisteredIntegrationEvent>
    {
        private readonly IEventMediator _eventMediator;

        public NotificationRegisteredIntegrationEventHandler(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        public override Task HandleAsync(NotificationRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            // var command = new RegisterReportCommand(integrationEvent.Notification);
            // await _eventMediator.PublishCommandAsync(command, cancellationToken);

            return Task.CompletedTask;
        }
    }
}