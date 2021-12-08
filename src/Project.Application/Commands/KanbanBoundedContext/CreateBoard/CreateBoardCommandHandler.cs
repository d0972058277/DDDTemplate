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

namespace Project.Application.Commands.KanbanBoundedContext.CreateBoard
{
    public class CreateBoardCommandHandler : ICommandHandler<CreateBoardCommand>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IEventMediator _eventMediator;

        public CreateBoardCommandHandler(IBoardRepository boardRepository, IEventMediator eventMediator)
        {
            _boardRepository = boardRepository;
            _eventMediator = eventMediator;
        }

        public async Task<Unit> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var board = await _boardRepository.FindAsync(request.BoardId, cancellationToken);

            if (board != null)
                throw new DomainException("已經有board了！");

            board = Board.Create(request.BoardId, request.Name, request.UserId);

            await _boardRepository.SaveAsync(board, cancellationToken);
            await _eventMediator.PublishDomainEventsAsync(board.GetAndClearDomainEvents(), cancellationToken);

            return Unit.Value;
        }
    }
}