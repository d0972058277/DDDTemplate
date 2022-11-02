using Architecture;

namespace Project.Application.Commands.ReadNotification
{
    public class ReadNotificationCommand : ICommand
    {
        public ReadNotificationCommand(Guid deviceId, Guid notificationId)
        {
            DeviceId = deviceId;
            NotificationId = notificationId;
        }

        public Guid DeviceId { get; }
        public Guid NotificationId { get; }
    }
}