using System.Collections.Generic;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void AddDomainEvent(IDomainEvent eventItem);
        void RemoveDomainEvent(IDomainEvent eventItem);
        void ClearDomainEvents();
        IEnumerable<IDomainEvent> GetAndClearDomainEvents();
    }
}