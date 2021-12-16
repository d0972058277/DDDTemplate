using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;

namespace Project.Application.Commands.KanbanBoundedContext.CreateWorkflow
{
    public class CreateWorkflowCommand : ICommand
    {
        public CreateWorkflowCommand(Guid workflowId, string name, Guid boardId)
        {
            WorkflowId = workflowId;
            Name = name;
            BoardId = boardId;
        }

        public Guid WorkflowId { get; }
        public string Name { get; }
        public Guid BoardId { get; }
    }
}