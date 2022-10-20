using Architecture;
using FluentAssertions;
using KnstArchitecture.SequentialGuid;
using Project.Domain.Aggregates.DeviceAggregate;
using Xunit;

namespace Project.UseCase.Test.Domain.Aggregates.DeviceAggregate
{
    public class NotificationTest
    {
        public static Notification CreateSuccessValue()
        {
            var id = SequentialGuid.NewGuid();
            var notification = Notification.Create(id);
            return notification;
        }

        [Fact]
        public void Create()
        {
            // Given
            var id = SequentialGuid.NewGuid();

            // When
            var notification = Notification.Create(id);

            // Then
            notification.Id.Should().Be(id);
            notification.ReadTime.Should().BeNull();
        }

        [Fact]
        public void Read()
        {
            // Given
            var notification = NotificationTest.CreateSuccessValue();

            // When
            notification.Read();

            // Then
            notification.ReadTime.Should().Be(SystemDateTime.UtcNow);
        }
    }
}