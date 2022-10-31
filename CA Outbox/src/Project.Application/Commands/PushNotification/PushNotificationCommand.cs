using Architecture;

namespace Project.Application.Commands.PushNotification
{
    public class PushNotificationCommand : ICommand
    {
        public PushNotificationCommand(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; }
    }
}