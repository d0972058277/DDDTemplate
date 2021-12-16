using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;

namespace Project.Domain.Events.KanbanBoundedContext
{
    public class WorkflowCreatedDomainEvent : IDomainEvent
    {
        public WorkflowCreatedDomainEvent(Guid workflowId)
        {
            WorkflowId = workflowId;
        }

        public Guid WorkflowId { get; }
    }
}