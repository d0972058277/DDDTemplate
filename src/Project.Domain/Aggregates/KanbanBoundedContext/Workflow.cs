using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Domain.Events.KanbanBoundedContext;

namespace Project.Domain.Aggregates.KanbanBoundedContext
{
    public class Workflow : Aggregate<Guid>
    {
        private Workflow(Guid id, string name, Guid boardId)
        {
            Id = id;
            Name = name;
            BoardId = boardId;
        }

        public string Name { get; private set; }
        public Guid BoardId { get; private set; }

        public static Workflow Create(Guid id, string name, Guid boardId)
        {
            var workflow = new Workflow(id, name, boardId);
            workflow.AddDomainEvent(new WorkflowCreatedDomainEvent(workflow.Id));
            return workflow;
        }
    }
}