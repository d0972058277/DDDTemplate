using MediatR;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent { }
}