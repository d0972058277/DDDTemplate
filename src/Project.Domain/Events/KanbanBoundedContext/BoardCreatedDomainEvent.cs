using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;

namespace Project.Domain.Events.KanbanBoundedContext
{
    public class BoardCreatedDomainEvent : IDomainEvent
    {
        public BoardCreatedDomainEvent(Guid boardId)
        {
            BoardId = boardId;
        }

        public Guid BoardId { get; }
    }
}