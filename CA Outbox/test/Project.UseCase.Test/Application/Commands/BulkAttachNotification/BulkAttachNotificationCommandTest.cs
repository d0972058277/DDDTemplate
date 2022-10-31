using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture;
using KnstArchitecture.SequentialGuid;
using Moq;
using Project.Application.Commands.BulkAttachNotification;
using Project.Application.Repositories;
using Project.Domain.Aggregates.DeviceAggregate;
using Project.Domain.Events;
using Project.UseCase.Test.Domain.Aggregates.DeviceAggregate;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.BulkAttachNotification
{
    public class BulkAttachNotificationCommandTest
    {
        [Fact]
        public async Task Handle()
        {
            // Given
            var devices = Enumerable.Range(1, 10).Select(i => DeviceTest.CreateRegistered()).ToList();
            var notificationId = SequentialGuid.NewGuid();

            var repository = new Mock<IDeviceRepository>();
            repository.Setup(m => m.FindsAsync(It.Is<IEnumerable<Guid>>(ids => ids.SequenceEqual(devices.Select(d => d.Id))), default)).ReturnsAsync(devices);
            var eventMediator = new Mock<IEventMediator>();

            var command = new BulkAttachNotificationCommand(devices.Select(d => d.Id), notificationId);
            var handler = new BulkAttachNotificationCommandHandler(repository.Object, eventMediator.Object);

            // When
            await handler.Handle(command, default);

            // Then
            repository.Verify(m => m.FindsAsync(It.Is<IEnumerable<Guid>>(ids => ids.SequenceEqual(devices.Select(d => d.Id))), default), Times.Once());
            repository.Verify(m => m.SavesAsync(It.Is<IEnumerable<Device>>(items => items.SequenceEqual(devices)), default), Times.Once());
            eventMediator.Verify(m => m.PublishDomainEventAsync(It.IsAny<NotificationBulkAttachedDomainEvent>(), default), Times.Once());
        }
    }
}