using Architecture;
using Microsoft.AspNetCore.Mvc;
using Project.WebApi.Controllers.NotificationEndpoint.Models;

namespace Project.WebApi.Controllers.NotificationEndpoint
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private IEventMediator _eventMediator;

        public NotificationController(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Guid>> Register([FromBody] RegisterPayload payload)
        {
            var notificationId = await _eventMediator.PublishCommandAsync(payload.ToCommand());
            return Ok(notificationId);
        }

        [HttpPost("Push")]
        public async Task<ActionResult> Push([FromBody] PushPayload payload)
        {
            await _eventMediator.PublishCommandAsync(payload.ToCommand());
            return Ok();
        }
    }
}