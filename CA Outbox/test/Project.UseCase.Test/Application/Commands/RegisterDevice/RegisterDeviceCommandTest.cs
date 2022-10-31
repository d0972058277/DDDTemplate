using System.Threading.Tasks;
using Architecture;
using Moq;
using Project.Application.Commands.RegisterDevice;
using Project.Application.Repositories;
using Project.Domain.Aggregates.DeviceAggregate;
using Project.Domain.Events;
using Project.UseCase.Test.Domain.Aggregates.DeviceAggregate;
using Xunit;

namespace Project.UseCase.Test.Application.Commands.RegisterDevice
{
    public class RegisterDeviceCommandTest
    {
        [Fact]
        public async Task Handle()
        {
            // Given
            var token = TokenTest.CreateSuccessValue();

            var repository = new Mock<IDeviceRepository>();
            var eventMediator = new Mock<IEventMediator>();

            var command = new RegisterDeviceCommand(token);
            var handler = new RegisterDeviceCommandHandler(repository.Object, eventMediator.Object);

            // When
            var deviceId = await handler.Handle(command, default);

            // Then
            repository.Verify(m => m.AddAsync(It.Is<Device>(d => d.Id == deviceId), default), Times.Once());
            eventMediator.Verify(m => m.PublishDomainEventAsync(It.Is<IDomainEvent>(e => e is DeviceRegisteredDomainEvent), default), Times.Once());
        }
    }
}