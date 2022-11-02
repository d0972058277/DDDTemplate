using Architecture;

namespace Project.Application.Commands.BulkAttachNotification
{
    public class BulkAttachNotificationCommand : ICommand
    {
        public BulkAttachNotificationCommand(IEnumerable<Guid> deviceIds, Guid notificationId)
        {
            DeviceIds = deviceIds;
            NotificationId = notificationId;
        }

        public IEnumerable<Guid> DeviceIds { get; }
        public Guid NotificationId { get; }
    }
}