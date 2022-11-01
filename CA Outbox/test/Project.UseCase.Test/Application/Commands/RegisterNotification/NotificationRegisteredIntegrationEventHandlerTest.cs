using System.Linq;
using System.Threading.Tasks;
using Architecture;
using KnstArchitecture.SequentialGuid;
using Moq;
using Project.Application.Commands.RegisterNotification;
using Project.Application.IntegrationEvents;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.RegisterNotification
{
    // NOTE: 這邊的 NotificationRegisteredIntegrationEventHandler 只是模擬，理應當要放到對應 BoundedContext 的專案當中
    public class NotificationRegisteredIntegrationEventHandlerTest
    {
        [Fact]
        public async Task Handle()
        {
            // Given
            var notificationId = SequentialGuid.NewGuid();
            var deviceIds = Enumerable.Range(1, 10).Select(i => SequentialGuid.NewGuid()).ToList();

            var eventMediator = new Mock<IEventMediator>();

            var integrationEvent = new NotificationRegisteredIntegrationEvent(notificationId, deviceIds);
            var handler = new NotificationRegisteredIntegrationEventHandler(eventMediator.Object);

            // When
            await handler.HandleAsync(integrationEvent, default);

            // Then
            // eventMediator.Verify(m => m.PublishCommandAsync(It.Is<RegisterReportCommand>(c => c.NotificationId == notificationId), default), Times.Once());
        }
    }
}