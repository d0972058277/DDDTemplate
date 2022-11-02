using Project.Application.Queries.ListNotifications;

namespace Project.WebApi.Controllers.DeviceEndpoint.Models
{
    public class ListNotificationsPayload
    {
        public Guid DeviceId { get; set; }

        public ListNotificationsQuery ToQuery()
        {
            return new ListNotificationsQuery(DeviceId);
        }
    }
}