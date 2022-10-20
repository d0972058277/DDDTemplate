using FluentAssertions;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Domain.Aggregates.NotificationAggregate;
using Xunit;

namespace Project.UseCase.Test.Domain.Aggregates.NotificationAggregate
{
    public class MessageTest
    {
        public static Message CreateSuccessValue()
        {
            var title = RandomString.Get(10);
            var body = RandomString.Get(10);
            var message = Message.Create(title, body);
            return message.Value;
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData("Title", null)]
        [InlineData(null, "Body")]
        [InlineData("Title", "Body")]
        public void Create_Success(string title, string body)
        {
            // Given

            // When
            var message = Message.Create(title, body);

            // Then
            message.IsSuccess.Should().BeTrue();
            message.Value.Title.Should().Be(title);
            message.Value.Body.Should().Be(body);
        }
    }
}