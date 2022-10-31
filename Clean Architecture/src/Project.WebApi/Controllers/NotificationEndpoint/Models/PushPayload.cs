using Project.Application.Commands.PushNotification;

namespace Project.WebApi.Controllers.NotificationEndpoint.Models
{
    public class PushPayload
    {
        public Guid NotificationId { get; set; }

        public PushNotificationCommand ToCommand()
        {
            return new PushNotificationCommand(NotificationId);
        }
    }
}