using Project.Application.Commands.ReadNotification;

namespace Project.WebApi.Controllers.DeviceEndpoint.Models
{
    public class ReadPayload
    {
        public Guid DeviceId { get; set; }
        public Guid NotificationId { get; set; }

        public ReadNotificationCommand ToCommand()
        {
            return new ReadNotificationCommand(DeviceId, NotificationId);
        }
    }
}