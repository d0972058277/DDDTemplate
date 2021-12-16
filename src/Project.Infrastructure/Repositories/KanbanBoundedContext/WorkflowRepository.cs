using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Application.Repositories.KanbanBoundedContext;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.Infrastructure.Repositories.KanbanBoundedContext
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly FakeDbContext _dbContext;

        public WorkflowRepository(FakeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Workflow> FindAsync(Guid workflowId, CancellationToken cancellationToken)
        {
            _dbContext.Workflows.TryGetValue(workflowId, out var workflow);
            return Task.FromResult(workflow);
        }

        public Task SaveAsync(Workflow workflow, CancellationToken cancellationToken)
        {
            _dbContext.Workflows.AddOrUpdate(workflow.Id, workflow, (id, oldWorkflow) => workflow);
            return Task.CompletedTask;
        }
    }
}