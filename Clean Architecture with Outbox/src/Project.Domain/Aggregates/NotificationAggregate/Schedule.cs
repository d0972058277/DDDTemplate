using Architecture;
using CSharpFunctionalExtensions;

namespace Project.Domain.Aggregates.NotificationAggregate
{
    public class Schedule : ValueObject
    {
        private Schedule(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Schedule> Create(DateTime schedule)
        {
            if (schedule <= SystemDateTime.UtcNow)
                return Result.Failure<Schedule>("Schedule 需超過當前時間");

            return new Schedule(schedule);
        }
    }
}