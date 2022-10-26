using CSharpFunctionalExtensions;

namespace Project.Domain.Aggregates.DeviceAggregate
{
    public class Token : ValueObject
    {
        private Token(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Token> Create(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Result.Failure<Token>("Token 不可為空或空字串");

            return new Token(token);
        }
    }
}