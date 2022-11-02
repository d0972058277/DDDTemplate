using System.Linq;
using System.Threading.Tasks;
using Architecture;
using KnstArchitecture.SequentialGuid;
using Moq;
using Project.Application.Commands.RegisterNotification;
using Project.Application.IntegrationEvents;
using Project.Domain.Events;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.RegisterNotification
{
    public class NotificationRegisteredDomainEventHandlerTest
    {
        [Fact]
        public async Task 推播登記後要登記報表資料()
        {
            // Given
            var notificationId = SequentialGuid.NewGuid();
            var deviceIds = Enumerable.Range(1, 10).Select(i => SequentialGuid.NewGuid()).ToList();

            var eventMediator = new Mock<IEventMediator>();

            var domainEvent = new NotificationRegisteredDomainEvent(notificationId, deviceIds);
            var handler = new NotificationRegisteredDomainEventHandler(eventMediator.Object);

            // When
            await handler.Handle(domainEvent, default);

            // Then
            eventMediator.Verify(m => m.PublishIntegrationEventAsync(It.Is<NotificationRegisteredIntegrationEvent>(c =>
                c.NotificationId == notificationId &&
                c.DeviceIds.SequenceEqual(deviceIds)), default), Times.Once());
        }
    }
}