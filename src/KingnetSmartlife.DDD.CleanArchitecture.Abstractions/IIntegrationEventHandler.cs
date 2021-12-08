using System.Threading;
using System.Threading.Tasks;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IIntegrationEventHandler { };

    public interface IIntegrationEventHandler<T> : IIntegrationEventHandler where T : IntegrationEvent
    {
        Task HandleAsync(T integrationEvent, CancellationToken cancellationToken);
    }
}