using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using MediatR;
using Project.Application.Repositories.KanbanBoundedContext;
using Project.Domain.Aggregates.KanbanBoundedContext;
using Project.Domain.Exceptions;

namespace Project.Application.Commands.KanbanBoundedContext.CreateWorkflow
{
    public class CreateWorkflowCommandHandler : ICommandHandler<CreateWorkflowCommand>
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IEventMediator _eventMediator;

        public CreateWorkflowCommandHandler(IWorkflowRepository workflowRepository, IEventMediator eventMediator)
        {
            _workflowRepository = workflowRepository;
            _eventMediator = eventMediator;
        }

        public async Task<Unit> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            var workflow = await _workflowRepository.FindAsync(request.WorkflowId, cancellationToken);

            if (workflow != null)
                throw new DomainException("已經有workflow了！");

            workflow = Workflow.Create(request.WorkflowId, request.Name, request.BoardId);

            await _workflowRepository.SaveAsync(workflow, cancellationToken);
            await _eventMediator.PublishDomainEventsAsync(workflow.GetAndClearDomainEvents(), cancellationToken);

            return Unit.Value;
        }
    }
}