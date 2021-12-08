using System;
using System.Linq;
using FluentAssertions;
using Project.Domain.Aggregates.KanbanBoundedContext;
using Project.Domain.Events.KanbanBoundedContext;
using Xunit;

namespace Project.UseCase.Test.Aggregates.KanbanBoundedContext
{
    public class BoardTest
    {
        [Fact]
        public void Create()
        {
            // Given
            var boardId = Guid.NewGuid();
            var boardName = "boardName";
            var userId = Guid.NewGuid();

            // When
            Board board = Board.Create(boardId, boardName, userId);

            // Then
            board.Id.Should().Be(boardId);
            board.Name.Should().Be(boardName);
            board.UserId.Should().Be(userId);
            board.DomainEvents.Should().NotBeEmpty();
            board.DomainEvents.Any(e => e is BoardCreatedDomainEvent).Should().BeTrue();
            board.DomainEvents.Single(e => e is BoardCreatedDomainEvent).As<BoardCreatedDomainEvent>().BoardId.Should().Be(boardId);
        }
    }
}