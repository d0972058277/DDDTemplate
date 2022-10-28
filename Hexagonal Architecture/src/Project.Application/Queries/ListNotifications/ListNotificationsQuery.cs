using Architecture;
using Project.Application.Queries.ListNotifications.Models;

namespace Project.Application.Queries.ListNotifications
{
    public class ListNotificationsQuery : IQuery<IReadOnlyList<Notification>>
    {
        public ListNotificationsQuery(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; }
    }
}