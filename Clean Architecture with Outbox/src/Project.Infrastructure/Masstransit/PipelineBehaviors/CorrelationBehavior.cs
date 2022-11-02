using Architecture;
using CorrelationId.Abstractions;
using MassTransit;

namespace Project.Infrastructure.Masstransit.PipelineBehaviors
{
    public class CorrelationBehavior<TMessage> : IFilter<ConsumeContext<TMessage>> where TMessage : IntegrationEvent
    {
        private readonly ICorrelationContextFactory _correlationContextFactory;

        public CorrelationBehavior(ICorrelationContextFactory correlationContextFactory)
        {
            _correlationContextFactory = correlationContextFactory;
        }

        public void Probe(ProbeContext context) { }

        public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
        {
            if (context.CorrelationId.HasValue)
                _correlationContextFactory.Create(context.CorrelationId.Value.ToString(), "CorrelationId");

            await next.Send(context);
        }
    }
}