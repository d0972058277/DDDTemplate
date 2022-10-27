namespace Project.WebApi.Controllers.DeviceEndpoint.Models
{
    public class ReadPayload
    {
        public Guid DeviceId { get; set; }
        public Guid NotificationId { get; set; }
    }
}