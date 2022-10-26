using System;
using System.Linq;
using Architecture;
using FluentAssertions;
using KnstArchitecture.SequentialGuid;
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

        [Fact]
        public void Read_正常流程()
        {
            // Given
            var device = DeviceTest.CreateRegistered();
            var notification = NotificationTest.CreateSuccessValue();
            device.Attach(notification);

            // When
            device.Read(notification.Id);

            // Then
            device.Notifications.Single(d => d.Id == notification.Id).ReadTime.Should().Be(SystemDateTime.UtcNow);
        }

        [Fact]
        public void Read_不包含對應的Notification時不做事()
        {
            // Given
            var device = DeviceTest.CreateRegistered();
            var notification = NotificationTest.CreateSuccessValue();
            device.Attach(notification);

            // When
            device.Read(SequentialGuid.NewGuid());

            // Then
            device.Notifications.All(n => n.ReadTime == default(DateTime?)).Should().BeTrue();
        }
    }
}