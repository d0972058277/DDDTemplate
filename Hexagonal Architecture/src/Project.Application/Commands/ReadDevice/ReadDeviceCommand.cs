using Architecture;

namespace Project.Application.Commands.ReadDevice
{
    public class ReadDeviceCommand : ICommand
    {
        public ReadDeviceCommand(Guid notificationId, Guid deviceId)
        {
            NotificationId = notificationId;
            DeviceId = deviceId;
        }

        public Guid NotificationId { get; }
        public Guid DeviceId { get; }
    }
}