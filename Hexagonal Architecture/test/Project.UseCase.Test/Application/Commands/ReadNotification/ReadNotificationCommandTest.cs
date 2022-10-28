using System.Threading.Tasks;
using Architecture;
using Moq;
using Project.Application.Commands.ReadNotification;
using Project.Application.Repositories;
using Project.Domain.Events;
using Project.UseCase.Test.Domain.Aggregates.DeviceAggregate;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.ReadNotification
{
    public class ReadNotificationCommandTest
    {
        [Fact]
        public async Task Handle()
        {
            // Given
            var device = DeviceTest.CreateRegistered();
            var notification = NotificationTest.CreateSuccessValue();
            device.Attach(notification);
            device.ClearDomainEvents();

            var repository = new Mock<IDeviceRepository>();
            repository.Setup(m => m.FindAsync(device.Id, notification.Id, default)).ReturnsAsync(device);
            var eventMediator = new Mock<IEventMediator>();

            var command = new ReadNotificationCommand(device.Id, notification.Id);
            var handler = new ReadNotificationCommandHandler(repository.Object, eventMediator.Object);

            // When
            await handler.Handle(command, default);

            // Then
            repository.Verify(m => m.FindAsync(device.Id, notification.Id, default), Times.Once());
            repository.Verify(m => m.SaveAsync(device, default), Times.Once());
            eventMediator.Verify(m => m.PublishDomainEventAsync(It.Is<IDomainEvent>(e => e is NotificationReadDomainEvent), default), Times.Once());
        }
    }
}