using MediatR;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }
}