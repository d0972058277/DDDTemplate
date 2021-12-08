using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Commands.KanbanBoundedContext.CreateBoard;
using Project.Application.Queries.KanbanBoundedContext.ListBoards;

namespace Project.WebApi.Controllers.Board
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IEventMediator _eventMediator;

        public BoardController(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(CreateBoardCommand command, CancellationToken cancellationToken)
        {
            await _eventMediator.PublishCommandAsync(command, cancellationToken);
            return Ok();
        }

        [HttpPost("List")]
        public async Task<ActionResult<IEnumerable<Domain.Aggregates.KanbanBoundedContext.Board>>> Create(ListBoardsQuery query, CancellationToken cancellationToken)
        {
            var boards = await _eventMediator.PublishQueryAsync(query, cancellationToken);
            return Ok(boards);
        }
    }
}