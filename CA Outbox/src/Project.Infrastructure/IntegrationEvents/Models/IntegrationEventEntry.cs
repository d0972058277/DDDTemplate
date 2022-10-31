using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Architecture;

namespace Project.Infrastructure.IntegrationEvents.Models
{
    public class IntegrationEventEntry
    {
#nullable disable warnings
        private IntegrationEventEntry() { }
#nullable enable warnings

        private IntegrationEventEntry(IntegrationEvent @event, Guid transactionId)
        {
            EventId = @event.Id;
            CreationTimestamp = @event.CreationTimestamp;
            EventTypeName = @event.GetType().FullName;
            Content = JsonSerializer.Serialize(@event, @event.GetType());
            State = IntegrationEventState.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId;
        }

        public Guid EventId { get; private set; }
        public string? EventTypeName { get; private set; }

        [NotMapped]
        public string? EventTypeShortName => EventTypeName?.Split('.').Last();
        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; } = default!;

        public IntegrationEventState State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTimestamp { get; private set; }
        public string Content { get; private set; }
        public Guid TransactionId { get; private set; }

        public static IntegrationEventEntry Create(IntegrationEvent @event, Guid transactionId)
        {
            return new IntegrationEventEntry(@event, transactionId);
        }

        public IntegrationEventEntry DeserializeJsonContent()
        {
            var type = IntegrationEventTypes.Instance.EventTypes.Find(t => t!.Name == EventTypeShortName)!;
            IntegrationEvent = (IntegrationEvent)JsonSerializer.Deserialize(Content, type)!;
            return this;
        }
    }
}