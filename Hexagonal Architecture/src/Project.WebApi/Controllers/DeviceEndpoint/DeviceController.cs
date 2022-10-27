using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Aggregates.DeviceAggregate;
using Project.Domain.Exceptions;
using Project.Infrastructure;
using Project.WebApi.Controllers.DeviceEndpoint.Models;

namespace Project.WebApi.Controllers.DeviceEndpoint
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ProjectDbContext _projectDbContext;

        public DeviceController(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Guid>> Register([FromBody] RegisterPayload payload)
        {
            using var transaction = await _projectDbContext.Database.BeginTransactionAsync();

            var token = Token.Create(payload.Token).Value;
            var device = Device.Register(token);
            
            _projectDbContext.Devices.Add(device);

            await _projectDbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(device.Id);
        }

        [HttpPost("Read")]
        public async Task<ActionResult> Read([FromBody] ReadPayload payload)
        {
            using var transaction = await _projectDbContext.Database.BeginTransactionAsync();

            var device = await _projectDbContext.Devices.FindAsync(payload.DeviceId).ThrowIfNullAsync(payload.DeviceId);
            await _projectDbContext.Entry(device).Collection(d => d.Notifications).Query().Where(n => n.Id == payload.NotificationId).LoadAsync();

            device.Read(payload.NotificationId);

            var notification = await _projectDbContext.Notifications.FindAsync(payload.NotificationId).ThrowIfNullAsync(payload.NotificationId);
            await _projectDbContext.Entry(notification).Collection(n => n.Devices).Query().Where(d => d.Id == payload.DeviceId).LoadAsync();

            notification.Read(payload.DeviceId);

            await _projectDbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok();
        }

        [HttpGet("List/{deviceId}")]
        public async Task<ActionResult<NotificationView[]>> List([FromRoute] Guid deviceId)
        {
            var sql = @"
SELECT
Notification.Id,
Notification.Message_Title AS Title,
Notification.Message_Body AS Body,
Notification.Schedule_Value AS Schedule,
Notification.PushedTime,
DeviceNotification.ReadTime
FROM Project.Device
JOIN Project.DeviceNotification ON DeviceNotification.DeviceId = Device.Id
JOIN Project.Notification ON Notification.Id = DeviceNotification.Id
WHERE TRUE
AND Device.Id = @deviceId;";
            var datas = await _projectDbContext.Database.GetDbConnection().QueryAsync<
                (
                    Guid id,
                    string title,
                    string body,
                    DateTime schedule,
                    DateTime? pushedTime,
                    DateTime? readTime
                )>(sql, new { deviceId });
            var notificationViews = datas.Select(data =>
                new NotificationView
                (
                    data.id,
                    new NotificationView.MessageDto
                    (
                        data.title,
                        data.body
                    ),
                    data.schedule,
                    data.pushedTime,
                    data.readTime
                )).ToArray();
            return Ok(notificationViews);
        }
    }
}