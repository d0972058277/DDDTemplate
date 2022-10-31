using System.Linq;
using System.Threading.Tasks;
using Architecture;
using Moq;
using Project.Application.Commands.ReadDevice;
using Project.Application.Repositories;
using Project.Domain.Events;
using Project.UseCase.Test.Domain.Aggregates.NotificationAggregate;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.ReadDevice
{
    public class ReadDeviceCommandTest
    {
        [Fact]
        public async Task Handle()
        {
            // Given
            var notification = NotificationTest.CreateRegistered();
            var device = notification.Devices.First();

            var repository = new Mock<INotificationRepository>();
            repository.Setup(m => m.FindAsync(notification.Id, device.Id, default)).ReturnsAsync(notification);
            var eventMediator = new Mock<IEventMediator>();

            var command = new ReadDeviceCommand(notification.Id, device.Id);
            var handler = new ReadDeviceCommandHandler(repository.Object, eventMediator.Object);

            // When
            await handler.Handle(command, default);

            // Then
            repository.Verify(m => m.FindAsync(notification.Id, device.Id, default), Times.Once());
            repository.Verify(m => m.SaveAsync(notification, default), Times.Once());
            eventMediator.Verify(m => m.PublishDomainEventAsync(It.Is<IDomainEvent>(e => e is DeviceReadDomainEvent), default), Times.Once());
        }
    }
}