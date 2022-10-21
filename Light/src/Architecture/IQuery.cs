using MediatR;

namespace Architecture
{
    public interface IBaseQuery : IBaseRequest { }
    public interface IQuery<out TResult> : IBaseQuery, IRequest<TResult> { }
}