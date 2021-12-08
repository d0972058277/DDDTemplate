using MediatR;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IBaseCommand : IBaseRequest { };
    public interface ICommand<out TResult> : IBaseCommand, IRequest<TResult> { }
    public interface ICommand : ICommand<Unit>, IRequest { }
}