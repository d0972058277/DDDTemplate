using FluentAssertions;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Domain.Aggregates.DeviceAggregate;
using Xunit;

namespace Project.UseCase.Test.Domain.Aggregates.DeviceAggregate
{
    public class TokenTest
    {
        public static Token CreateSuccessValue()
        {
            var value = RandomString.Get(10);
            var token = Token.Create(value);
            return token.Value;
        }

        [Fact]
        public void Create_Success()
        {
            // Given
            var value = RandomString.Get(10);

            // When
            var token = Token.Create(value);

            // Then
            token.IsSuccess.Should().BeTrue();
            token.Value.Value.Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Create_Failure(string value)
        {
            // Given

            // When
            var token = Token.Create(value);

            // Then
            token.IsFailure.Should().BeTrue();
        }
    }
}