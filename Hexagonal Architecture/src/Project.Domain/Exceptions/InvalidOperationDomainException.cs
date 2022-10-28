using System.Runtime.Serialization;

namespace Project.Domain.Exceptions
{
    public class InvalidOperationDomainException : DomainException
    {
        public InvalidOperationDomainException() { }

        public InvalidOperationDomainException(string message) : base(message) { }

        public InvalidOperationDomainException(string message, Exception innerException) : base(message, innerException) { }

        protected InvalidOperationDomainException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}