using MediatR;

namespace Architecture
{
    public interface IBaseCommand : IBaseRequest { };
    public interface ICommand<out TResult> : IBaseCommand, IRequest<TResult> { }
    public interface ICommand : ICommand<Unit>, IRequest { }
}