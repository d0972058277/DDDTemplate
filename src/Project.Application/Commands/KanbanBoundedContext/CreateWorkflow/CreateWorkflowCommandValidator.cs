using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Project.Application.Commands.KanbanBoundedContext.CreateWorkflow
{
    public class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
    {
        public CreateWorkflowCommandValidator()
        {
            RuleFor(command => command.WorkflowId).NotEqual(Guid.Empty);
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.BoardId).NotEqual(Guid.Empty);
        }
    }
}