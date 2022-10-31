using System.Runtime.Serialization;

namespace Project.Domain.Exceptions
{
    public class ValidationDomainException : DomainException
    {
        public ValidationDomainException() { }

        public ValidationDomainException(string message) : base(message) { }

        public ValidationDomainException(string message, Exception innerException) : base(message, innerException) { }

        protected ValidationDomainException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}