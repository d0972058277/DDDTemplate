using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Application.Repositories.KanbanBoundedContext;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.UseCase.Test
{
    public abstract class FakeWorkflowRepository : IWorkflowRepository
    {
        public Dictionary<Guid, Workflow> DataSet = new Dictionary<Guid, Workflow>();
        public abstract Task<Workflow> FindAsync(Guid workflowId, CancellationToken cancellationToken);
        public abstract Task SaveAsync(Workflow workflow, CancellationToken cancellationToken);

        public void Init(Workflow workflow)
        {
            DataSet.TryAdd(workflow.Id, workflow);
        }
    }
}