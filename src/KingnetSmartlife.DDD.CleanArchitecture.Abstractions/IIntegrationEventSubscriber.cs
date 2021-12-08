using System.Threading;
using System.Threading.Tasks;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IIntegrationEventSubscriber
    {
        Task HandleEventAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken) where TIntegrationEvent : IntegrationEvent;
    }
}