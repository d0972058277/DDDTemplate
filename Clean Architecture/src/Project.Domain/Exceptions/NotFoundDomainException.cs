using System.Runtime.Serialization;
using Architecture;

namespace Project.Domain.Exceptions
{
    public class NotFoundDomainException : DomainException
    {
        public NotFoundDomainException() { }
        public NotFoundDomainException(string message) : base(message) { }
        public NotFoundDomainException(string message, Exception innerException) : base(message, innerException) { }
        protected NotFoundDomainException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public static class NotFoundedDomainExceptionExtensions
    {
        public static async Task<TAggregate> ThrowIfNullAsync<TAggregate, TId>(this Task<TAggregate?> task, TId id) where TAggregate : IAggregateRoot
        {
            var aggregate = await task;

            if (aggregate is null)
                throw new NotFoundDomainException($"找不到指定 Id {id} 的 {typeof(TAggregate).Name} Aggregate");
            else
                return aggregate;
        }

        public static async Task<TAggregate> ThrowIfNullAsync<TAggregate, TId>(this ValueTask<TAggregate?> task, TId id) where TAggregate : IAggregateRoot
        {
            var aggregate = await task;

            if (aggregate is null)
                throw new NotFoundDomainException($"找不到指定 Id {id} 的 {typeof(TAggregate).Name} Aggregate");
            else
                return aggregate;
        }
    }
}