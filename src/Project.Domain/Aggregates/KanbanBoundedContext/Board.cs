using System;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Domain.Events.KanbanBoundedContext;

namespace Project.Domain.Aggregates.KanbanBoundedContext
{
    public class Board : Aggregate<Guid>
    {
        private string test;

        private Board(Guid id, string name, Guid userId)
        {
            Id = id;
            Name = name;
            UserId = userId;
        }

        public string Name { get; private set; }
        public Guid UserId { get; private set; }

        public static Board Create(Guid boardId, string boardName, Guid userId)
        {
            var board = new Board(boardId, boardName, userId);
            board.AddDomainEvent(new BoardCreatedDomainEvent(board.Id));
            return board;
        }
    }
}