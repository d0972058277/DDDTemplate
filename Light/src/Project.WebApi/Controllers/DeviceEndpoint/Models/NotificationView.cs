namespace Project.WebApi.Controllers.DeviceEndpoint.Models
{
    public class NotificationView
    {
        public NotificationView(Guid id, MessageDto message, DateTime schedule, DateTime? pushedTime, DateTime? readTime)
        {
            Id = id;
            Message = message;
            Schedule = schedule;
            PushedTime = pushedTime;
            ReadTime = readTime;
        }

        public Guid Id { get; }

        public MessageDto Message { get; }

        public DateTime Schedule { get; }

        public DateTime? PushedTime { get; }
        public DateTime? ReadTime { get; }

        public class MessageDto
        {
            public MessageDto(string title, string body)
            {
                Title = title;
                Body = body;
            }

            public string Title { get; }
            public string Body { get; }
        }
    }
}