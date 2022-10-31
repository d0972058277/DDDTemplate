using System.Threading.Tasks;
using Architecture;
using KnstArchitecture.SequentialGuid;
using Moq;
using Project.Application.Commands.ReadDevice;
using Project.Domain.Events;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.RegisterDevice
{
    public class NotificationReadDomainEventHandlerTest
    {
        [Fact]
        public async Task 裝置內的推播已讀後要同步推播進行裝置已讀()
        {
            // Given
            var deviceId = SequentialGuid.NewGuid();
            var notificationId = SequentialGuid.NewGuid();

            var eventMediator = new Mock<IEventMediator>();

            var domainEvent = new NotificationReadDomainEvent(deviceId, notificationId);
            var handler = new NotificationReadDomainEventHandler(eventMediator.Object);

            // When
            await handler.Handle(domainEvent, default);

            // Then
            eventMediator.Verify(m => m.PublishCommandAsync(It.Is<ReadDeviceCommand>(c =>
                c.NotificationId == notificationId &&
                c.DeviceId == deviceId), default), Times.Once());
        }
    }
}