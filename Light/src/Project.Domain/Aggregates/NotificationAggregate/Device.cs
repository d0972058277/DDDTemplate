using Architecture;
using CSharpFunctionalExtensions;

namespace Project.Domain.Aggregates.NotificationAggregate
{
    public class Device : Entity<Guid>
    {
        public Device(Guid id) : base(id) { }

        public DateTime? ReadTime { get; private set; }

        public static Device Create(Guid id)
        {
            return new Device(id);
        }

        public void Read()
        {
            ReadTime = SystemDateTime.UtcNow;
        }
    }
}