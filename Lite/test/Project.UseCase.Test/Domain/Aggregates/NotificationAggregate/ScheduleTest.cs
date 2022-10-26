using System;
using System.Collections.Generic;
using Architecture;
using FluentAssertions;
using Project.Domain.Aggregates.NotificationAggregate;
using Xunit;

namespace Project.UseCase.Test.Domain.Aggregates.NotificationAggregate
{
    public class ScheduleTest
    {
        public static Schedule CreateSuccessValue()
        {
            var value = SystemDateTime.UtcNow.AddTicks(1);
            var schedule = Schedule.Create(value);
            return schedule.Value;
        }

        [Fact]
        public void Create_Success()
        {
            // Given
            var value = SystemDateTime.UtcNow.AddTicks(1);

            // When
            var schedule = Schedule.Create(value);

            // Then
            schedule.IsSuccess.Should().BeTrue();
            schedule.Value.Value.Should().Be(value);
        }

        public static IEnumerable<object[]> Create_Failure_Datas
        {
            get
            {
                yield return new object[] { SystemDateTime.UtcNow };
                yield return new object[] { SystemDateTime.UtcNow.AddTicks(-1) };
            }
        }

        [Theory]
        [MemberData(nameof(Create_Failure_Datas))]
        public void Create_Failure(DateTime value)
        {
            // Given

            // When
            var schedule = Schedule.Create(value);

            // Then
            schedule.IsFailure.Should().BeTrue();
        }
    }
}