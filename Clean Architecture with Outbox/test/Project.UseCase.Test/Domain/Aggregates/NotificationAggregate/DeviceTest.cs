using Architecture;
using FluentAssertions;
using KnstArchitecture.SequentialGuid;
using Project.Domain.Aggregates.NotificationAggregate;
using Xunit;

namespace Project.UseCase.Test.Domain.Aggregates.NotificationAggregate
{
    public class DeviceTest
    {
        public static Device CreateSuccessValue()
        {
            var deviceId = SequentialGuid.NewGuid();
            var device = Device.Create(deviceId);
            return device;
        }

        [Fact]
        public void Create()
        {
            // Given
            var deviceId = SequentialGuid.NewGuid();

            // When
            var device = Device.Create(deviceId);

            // Then
            device.Id.Should().Be(deviceId);
            device.ReadTime.Should().BeNull();
        }

        [Fact]
        public void Read()
        {
            // Given
            var device = DeviceTest.CreateSuccessValue();

            // When
            device.Read();

            // Then
            device.ReadTime.Should().Be(SystemDateTime.UtcNow);
        }
    }
}