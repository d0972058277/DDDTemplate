using MassTransit;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public abstract class IntegrationEventHandler<T> : IConsumer<T> where T : IntegrationEvent
    {
        public virtual Task Consume(ConsumeContext<T> context)
        {
            return HandleAsync(context.Message, context.CancellationToken);
        }

        public abstract Task HandleAsync(T integrationEvent, CancellationToken cancellationToken);
    }
}