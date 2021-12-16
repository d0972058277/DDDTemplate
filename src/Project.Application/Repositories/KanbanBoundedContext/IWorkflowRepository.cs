using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.Application.Repositories.KanbanBoundedContext
{
    public interface IWorkflowRepository : IAggregateRepository<Workflow>
    {
        Task<Workflow> FindAsync(Guid workflowId, CancellationToken cancellationToken);
        Task SaveAsync(Workflow workflow, CancellationToken cancellationToken);
    }
}