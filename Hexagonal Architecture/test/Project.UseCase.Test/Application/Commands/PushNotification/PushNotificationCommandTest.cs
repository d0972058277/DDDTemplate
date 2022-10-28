using System.Threading.Tasks;
using Architecture;
using Moq;
using Project.Application.Commands.PushNotification;
using Project.Application.Repositories;
using Project.Application.Services;
using Project.Domain.Events;
using Project.UseCase.Test.Domain.Aggregates.NotificationAggregate;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.PushNotification
{
    public class PushNotificationCommandTest
    {
        [Fact]
        public async Task Handle()
        {
            // Given
            var notification = NotificationTest.CreateRegistered();

            var repository = new Mock<INotificationRepository>();
            repository.Setup(m => m.FindAsync(notification.Id, default)).ReturnsAsync(notification);
            var eventMediator = new Mock<IEventMediator>();
            var service = new Mock<IPushNotificationService>();

            var command = new PushNotificationCommand(notification.Id);
            var handler = new PushNotificationCommandHandler(repository.Object, eventMediator.Object, service.Object);

            // When
            await handler.Handle(command, default);

            // Then
            repository.Verify(m => m.FindAsync(notification.Id, default), Times.Once());
            repository.Verify(m => m.SaveAsync(notification, default), Times.Once());
            service.Verify(m => m.ExecuteAsync(notification, default), Times.Once());
            eventMediator.Verify(m => m.PublishDomainEventAsync(It.Is<IDomainEvent>(e => e is NotificationPushedDomainEvent), default), Times.Once());
        }
    }
}