using System.Linq;
using FluentAssertions;
using Project.Domain.Aggregates.DeviceAggregate;
using Xunit;

namespace Project.UseCase.Test.Domain.Aggregates.DeviceAggregate
{
    public class DeviceTest
    {
        public static Device CreateRegistered()
        {
            var token = TokenTest.CreateSuccessValue();
            var device = Device.Register(token);
            return device;
        }

        [Fact]
        public void Register()
        {
            // Given
            var token = TokenTest.CreateSuccessValue();

            // When
            var device = Device.Register(token);

            // Then
            device.Id.Should().NotBeEmpty();
            device.Token.Should().Be(token);
        }

        [Fact]
        public void Attach_正常流程()
        {
            // Given
            var device = DeviceTest.CreateRegistered();
            var notification = NotificationTest.CreateSuccessValue();

            // When
            device.Attach(notification);

            // Then
            device.Notifications.Contains(notification);
        }

        [Fact]
        public void Attach_有重複推播時不再附加()
        {
            // Given
            var device = DeviceTest.CreateRegistered();
            var notification = NotificationTest.CreateSuccessValue();
            device.Attach(notification);

            // When
            device.Attach(notification);

            // Then
            device.Notifications.Count.Should().Be(1);
        }
    }
}