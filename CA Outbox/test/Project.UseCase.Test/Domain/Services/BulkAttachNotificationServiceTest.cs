using System.Linq;
using FluentAssertions;
using KnstArchitecture.SequentialGuid;
using Project.Domain.Aggregates.DeviceAggregate;
using Project.Domain.Services;
using Project.UseCase.Test.Domain.Aggregates.DeviceAggregate;
using Xunit;

namespace Project.UseCase.Test.Domain.Services
{
    public class BulkAttachNotificationServiceTest
    {
        [Fact]
        public void Execute()
        {
            // Given
            var devices = Enumerable.Range(1, 10).Select(i => DeviceTest.CreateRegistered()).ToList();
            var notificationId = SequentialGuid.NewGuid();

            // When
            var domainEvent = BulkAttachNotificationService.Execute(devices, notificationId);

            // Then
            devices.All(d => d.Notifications.Contains(Notification.Create(notificationId))).Should().BeTrue();
            domainEvent.NotificationAttachedDomainEvents.Select(e => e.DeviceId).Should().BeEquivalentTo(devices.Select(d => d.Id));
            domainEvent.NotificationAttachedDomainEvents.All(e => e.NotificationId == notificationId).Should().BeTrue();
        }
    }
}