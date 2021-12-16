using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Project.Application.Commands.KanbanBoundedContext.CreateBoard;
using Project.Domain.Aggregates.KanbanBoundedContext;
using Project.Domain.Events.KanbanBoundedContext;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.UseCase.Test.Commands.KanbanBoundedContext.CreateBoard
{
    public class CreateBoardCommandTest
    {
        Mock<FakeBoardRepository> _mockBoardRepository;
        Mock<FakeEventMediator> _mockEventMediator;

        public CreateBoardCommandTest(Mock<FakeBoardRepository> mockNotificationRepository, Mock<FakeEventMediator> mockEventMediator)
        {
            _mockBoardRepository = mockNotificationRepository;
            _mockEventMediator = mockEventMediator;
        }

        FakeBoardRepository BoardRepository => _mockBoardRepository.Object;

        IEventMediator EventMediator => _mockEventMediator.Object;

        [Fact]
        public async Task CreateBoardCommand成功創建Board()
        {
            // Given
            var boardId = Guid.NewGuid();
            var name = "boardName";
            var userId = Guid.NewGuid();

            var command = new CreateBoardCommand(boardId, name, userId);
            var handler = new CreateBoardCommandHandler(BoardRepository, EventMediator);
            var cancellationToken = default(CancellationToken);

            // When
            await handler.Handle(command, cancellationToken);

            // Then
            _mockBoardRepository.Verify(m => m.FindAsync(boardId, cancellationToken), Times.Once());
            _mockBoardRepository.Verify(m => m.SaveAsync(It.IsAny<Board>(), cancellationToken), Times.Once());
            _mockEventMediator.Verify(m => m.PublishDomainEventAsync(It.Is<IDomainEvent>(e => e is BoardCreatedDomainEvent), cancellationToken), Times.Once());
        }

        [Fact]
        public async Task CreateBoardCommand不能重複創建相同的Board()
        {
            // Given
            var boardId = Guid.NewGuid();
            var name = "boardName";
            var userId = Guid.NewGuid();
            var board = Board.Create(boardId, name, userId);
            board.ClearDomainEvents();
            BoardRepository.Init(board);

            var command = new CreateBoardCommand(boardId, name, userId);
            var handler = new CreateBoardCommandHandler(BoardRepository, EventMediator);
            var cancellationToken = default(CancellationToken);

            // When
            Func<Task<Unit>> func = () => handler.Handle(command, cancellationToken);

            // Then
            await func.Should().ThrowAsync<DomainException>();
        }
    }
}