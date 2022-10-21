using KnstArchitecture.SequentialGuid;

namespace Architecture
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = SequentialGuid.NewGuid();
            CreationTimestamp = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public DateTime CreationTimestamp { get; private set; }
    }
}