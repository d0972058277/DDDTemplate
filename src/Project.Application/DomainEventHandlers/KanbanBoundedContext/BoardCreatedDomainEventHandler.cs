using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Application.Commands.KanbanBoundedContext.CreateWorkflow;
using Project.Domain.Events.KanbanBoundedContext;

namespace Project.Application.DomainEventHandlers.KanbanBoundedContext
{
    public class BoardCreatedDomainEventHandler : IDomainEventHandler<BoardCreatedDomainEvent>
    {
        private readonly IEventMediator _eventMediator;

        public BoardCreatedDomainEventHandler(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        public async Task Handle(BoardCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var workflowId = Guid.NewGuid();
            var name = "default";
            var boardId = notification.BoardId;
            var command = new CreateWorkflowCommand(workflowId, name, boardId);

            await _eventMediator.PublishCommandAsync(command, cancellationToken);
        }
    }
}