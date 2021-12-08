using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using Project.Application.Commands.KanbanBoundedContext.CreateBoard;
using Project.Domain.Aggregates.KanbanBoundedContext;
using Project.Domain.Events.KanbanBoundedContext;
using Xunit;

namespace Project.UseCase.Test.Commands.KanbanBoundedContext.CreateBoard
{
    public class CreateBoardCommandTest
    {
        Mock<FakeBoardRepository> _mockNotificationRepository;
        Mock<FakeEventMediator> _mockEventMediator;

        public CreateBoardCommandTest(Mock<FakeBoardRepository> mockNotificationRepository, Mock<FakeEventMediator> mockEventMediator)
        {
            _mockNotificationRepository = mockNotificationRepository;
            _mockEventMediator = mockEventMediator;
        }

        FakeBoardRepository BoardRepository => _mockNotificationRepository.Object;

        IEventMediator EventMediator => _mockEventMediator.Object;

        [Fact]
        public async Task CreateBoard()
        {
            // Given
            var boardId = Guid.NewGuid();
            var boardName = "boardName";
            var userId = Guid.NewGuid();

            var command = new CreateBoardCommand(boardId, boardName, userId);
            var handler = new CreateBoardCommandHandler(BoardRepository, EventMediator);
            var cancellationToken = default(CancellationToken);

            // When
            await handler.Handle(command, cancellationToken);

            // Then
            _mockNotificationRepository.Verify(m => m.FindAsync(boardId, cancellationToken), Times.Once());
            _mockNotificationRepository.Verify(m => m.SaveAsync(It.IsAny<Board>(), cancellationToken), Times.Once());
            _mockEventMediator.Verify(m => m.PublishDomainEventAsync(It.Is<IDomainEvent>(e => e is BoardCreatedDomainEvent), cancellationToken), Times.Once());
        }
    }
}