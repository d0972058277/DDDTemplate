using CSharpFunctionalExtensions;

namespace Project.Domain.Aggregates.NotificationAggregate
{
    public class Message : ValueObject
    {
        private Message(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public string Title { get; }
        public string Body { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Body;
        }

        public static Result<Message> Create(string title, string body)
        {
            return new Message(title, body);
        }
    }
}