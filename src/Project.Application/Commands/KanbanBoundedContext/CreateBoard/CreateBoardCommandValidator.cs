using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Project.Application.Commands.KanbanBoundedContext.CreateBoard
{
    public class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
    {
        public CreateBoardCommandValidator()
        {
            RuleFor(command => command.BoardId).NotEqual(Guid.Empty);
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.UserId).NotEqual(Guid.Empty);
        }
    }
}