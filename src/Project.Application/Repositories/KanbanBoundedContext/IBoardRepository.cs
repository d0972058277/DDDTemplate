using System;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.Application.Repositories.KanbanBoundedContext
{
    public interface IBoardRepository : IAggregateRepository<Board>
    {
        Task<Board> FindAsync(Guid boardId, CancellationToken cancellationToken);
        Task SaveAsync(Board board, CancellationToken cancellationToken);
    }
}