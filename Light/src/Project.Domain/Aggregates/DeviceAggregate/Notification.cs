using Architecture;
using CSharpFunctionalExtensions;

namespace Project.Domain.Aggregates.DeviceAggregate
{
    public class Notification : Entity<Guid>
    {
        public Notification(Guid id) : base(id) { }

        public DateTime? ReadTime { get; private set; }

        public static Notification Create(Guid id)
        {
            return new Notification(id);
        }

        public void Read()
        {
            ReadTime = SystemDateTime.UtcNow;
        }
    }
}