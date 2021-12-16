using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Commands.KanbanBoundedContext.CreateWorkflow;

namespace Project.WebApi.Controllers.Workflow
{
    [ApiController]
    [Route("[controller]")]
    public class WorkflowController : ControllerBase
    {
        private readonly IEventMediator _eventMediator;

        public WorkflowController(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(CreateWorkflowCommand command, CancellationToken cancellationToken)
        {
            await _eventMediator.PublishCommandAsync(command, cancellationToken);
            return Ok();
        }
    }
}