using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Application.Repositories.KanbanBoundedContext;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.Infrastructure.Repositories.KanbanBoundedContext
{
    public class ListBoardsQueryRepository : IListBoardsQueryRepository
    {
        private readonly FakeDbContext _dbContext;

        public ListBoardsQueryRepository(FakeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Board>> ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return _dbContext.Boards.Values.ToList();
        }
    }
}