using System.Linq;
using System.Threading.Tasks;
using Architecture;
using KnstArchitecture.SequentialGuid;
using Moq;
using Project.Application.Commands.RegisterNotification;
using Project.Application.Repositories;
using Project.Domain.Aggregates.NotificationAggregate;
using Project.Domain.Events;
using Project.UseCase.Test.Domain.Aggregates.NotificationAggregate;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.RegisterNotification
{
    public class RegisterNotificationCommandTest
    {
        [Fact]
        public async Task Handle()
        {
            // Given
            var message = MessageTest.CreateSuccessValue();
            var schedule = ScheduleTest.CreateSuccessValue();
            var deviceIds = Enumerable.Range(1, 10).Select(i => SequentialGuid.NewGuid()).ToList();

            var repository = new Mock<INotificationRepository>();
            var eventMediator = new Mock<IEventMediator>();

            var command = new RegisterNotificationCommand(message, schedule, deviceIds);
            var handler = new RegisterNotificationCommandHandler(repository.Object, eventMediator.Object);

            // When
            var notificationId = await handler.Handle(command, default);

            // Then
            repository.Verify(m => m.AddAsync(It.Is<Notification>(n => n.Id == notificationId), default), Times.Once());
            eventMediator.Verify(m => m.PublishDomainEventAsync(It.Is<IDomainEvent>(e => e is NotificationRegisteredDomainEvent), default), Times.Once());
        }
    }
}