using MediatR;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IBaseQuery : IBaseRequest { }
    public interface IQuery<out TResult> : IBaseQuery, IRequest<TResult> { }
}