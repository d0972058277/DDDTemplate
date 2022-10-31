using Architecture;
using Microsoft.AspNetCore.Mvc;
using Project.WebApi.Controllers.DeviceEndpoint.Models;

namespace Project.WebApi.Controllers.DeviceEndpoint
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private IEventMediator _eventMediator;

        public DeviceController(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Guid>> Register([FromBody] RegisterPayload payload)
        {
            var deviceId = await _eventMediator.PublishCommandAsync(payload.ToCommand());
            return Ok(deviceId);
        }

        [HttpPost("Read")]
        public async Task<ActionResult> Read([FromBody] ReadPayload payload)
        {
            await _eventMediator.PublishCommandAsync(payload.ToCommand());
            return Ok();
        }

        [HttpGet("List/{DeviceId}")]
        public async Task<ActionResult<Application.Queries.ListNotifications.Models.Notification[]>> List([FromRoute] ListNotificationsPayload payload)
        {
            var notifications = await _eventMediator.PublishQueryAsync(payload.ToQuery());
            return Ok(notifications);
        }
    }
}