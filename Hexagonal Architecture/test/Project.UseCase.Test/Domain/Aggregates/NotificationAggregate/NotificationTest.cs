using System;
using System.Linq;
using Architecture;
using FluentAssertions;
using KnstArchitecture.SequentialGuid;
using Project.Domain.Aggregates.NotificationAggregate;
using Project.Domain.Events;
using Xunit;

namespace Project.UseCase.Test.Domain.Aggregates.NotificationAggregate
{
    public class NotificationTest
    {
        public static Notification CreateRegistered()
        {
            var message = MessageTest.CreateSuccessValue();
            var schedule = ScheduleTest.CreateSuccessValue();
            var devices = Enumerable.Range(1, 10).Select(i => DeviceTest.CreateSuccessValue()).ToHashSet();
            var notification = Notification.Register(message, schedule, devices);
            notification.ClearDomainEvents();
            return notification;
        }

        [Fact]
        public void Register()
        {
            // Given
            var message = MessageTest.CreateSuccessValue();
            var schedule = ScheduleTest.CreateSuccessValue();
            var devices = Enumerable.Range(1, 10).Select(i => DeviceTest.CreateSuccessValue()).ToHashSet();

            // When
            var notification = Notification.Register(message, schedule, devices);

            // Then
            notification.Message.Should().Be(message);
            notification.Schedule.Should().Be(schedule);
            notification.Devices.Should().BeEquivalentTo(devices);
            notification.PushedTime.Should().BeNull();
            notification.DomainEvents.Single().As<NotificationRegisteredDomainEvent>().NotificationId.Should().Be(notification.Id);
            notification.DomainEvents.Single().As<NotificationRegisteredDomainEvent>().DeviceIds.Should().BeEquivalentTo(devices.Select(d => d.Id).ToList());
        }

        [Fact]
        public void Push()
        {
            // Given
            var notification = NotificationTest.CreateRegistered();

            // When
            notification.Push();

            // Then
            notification.PushedTime.Should().Be(SystemDateTime.UtcNow);
            notification.DomainEvents.Single().As<NotificationPushedDomainEvent>().NotificationId.Should().Be(notification.Id);
        }

        [Fact]
        public void Read_正常流程()
        {
            // Given
            var notification = NotificationTest.CreateRegistered();
            var deviceId = notification.Devices.First().Id;

            // When
            notification.Read(deviceId);

            // Then
            notification.Devices.Single(d => d.Id == deviceId).ReadTime.Should().Be(SystemDateTime.UtcNow);
            notification.DomainEvents.Single().As<DeviceReadDomainEvent>().NotificationId.Should().Be(notification.Id);
            notification.DomainEvents.Single().As<DeviceReadDomainEvent>().DeviceId.Should().Be(deviceId);
        }

        [Fact]
        public void Read_不包含對應的Device時不做事()
        {
            // Given
            var notification = NotificationTest.CreateRegistered();
            var deviceId = SequentialGuid.NewGuid();

            // When
            notification.Read(deviceId);

            // Then
            notification.Devices.All(d => d.ReadTime == default(DateTime?)).Should().BeTrue();
            notification.DomainEvents.Should().BeEmpty();
        }
    }
}