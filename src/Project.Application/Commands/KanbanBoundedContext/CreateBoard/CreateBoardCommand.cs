using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;

namespace Project.Application.Commands.KanbanBoundedContext.CreateBoard
{
    public class CreateBoardCommand : ICommand
    {
        public CreateBoardCommand(Guid boardId, string name, Guid userId)
        {
            BoardId = boardId;
            Name = name;
            UserId = userId;
        }

        public Guid BoardId { get; }
        public string Name { get; }
        public Guid UserId { get; }
    }
}