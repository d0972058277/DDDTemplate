using Architecture;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.Application.Commands.RegisterNotification
{
    public class RegisterNotificationCommand : ICommand<Guid>
    {
        public RegisterNotificationCommand(Message message, Schedule schedule, IEnumerable<Guid> deviceIds)
        {
            Message = message;
            Schedule = schedule;
            DeviceIds = deviceIds;
        }

        public Message Message { get; }
        public Schedule Schedule { get; }
        public IEnumerable<Guid> DeviceIds { get; }
    }
}