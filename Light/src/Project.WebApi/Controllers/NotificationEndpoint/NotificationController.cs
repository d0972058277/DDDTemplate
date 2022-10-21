using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Aggregates.NotificationAggregate;
using Project.Infrastructure;
using Project.WebApi.Controllers.NotificationEndpoint.Models;

namespace Project.WebApi.Controllers.NotificationEndpoint
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ProjectDbContext _projectDbContext;

        public NotificationController(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Guid>> Register([FromBody] RegisterPayload payload)
        {
            await _projectDbContext.Database.BeginTransactionAsync();

            var message = Message.Create(payload.Message.Title, payload.Message.Body);
            var schedule = Schedule.Create(payload.Schedule);
            var deviceEntities = payload.DeviceIds.Select(id => Device.Create(id)).ToHashSet();
            var notification = Notification.Register(message.Value, schedule.Value, deviceEntities);

            _projectDbContext.Notifications.Add(notification);

            var devices = await _projectDbContext.Devices.Where(d => payload.DeviceIds.Contains(d.Id)).ToListAsync();
            // NOTE: 由於 ef.core 會透過記憶體位址來判定 entity 是否相同，所以每個 notification entity 需要重新建立
            devices.ForEach(device => device.Attach(Domain.Aggregates.DeviceAggregate.Notification.Create(notification.Id)));

            // TODO: 加入 Push Application Service 帶入 External System
            notification.Push();

            await _projectDbContext.SaveChangesAsync();
            await _projectDbContext.Database.CommitTransactionAsync();

            return Ok(notification.Id);
        }
    }
}