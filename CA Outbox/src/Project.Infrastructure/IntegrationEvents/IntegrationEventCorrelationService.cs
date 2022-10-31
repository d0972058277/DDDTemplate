using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.Extensions.Options;

namespace Project.Infrastructure.IntegrationEvents
{
    public class IntegrationEventCorrelationService
    {
        private readonly ICorrelationContextFactory _correlationContextFactory;
        private readonly ICorrelationContextAccessor _correlationContext;
        private readonly CorrelationIdOptions _correlationIdOptions;

        public IntegrationEventCorrelationService(ICorrelationContextFactory correlationContextFactory, ICorrelationContextAccessor correlationContext, IOptions<CorrelationIdOptions> correlationIdOptions)
        {
            _correlationContextFactory = correlationContextFactory;
            _correlationContext = correlationContext;
            _correlationIdOptions = correlationIdOptions.Value;
        }

        public Guid CorrelationId
        {
            get
            {
                var hadCorrelationId = Guid.TryParse(_correlationContext.CorrelationContext?.CorrelationId, out var correlationId);

                if (hadCorrelationId)
                    return correlationId;

                correlationId = Guid.NewGuid();
                _correlationContextFactory.Create(correlationId.ToString(), _correlationIdOptions.RequestHeader);
                return correlationId;
            }
        }
    }
}