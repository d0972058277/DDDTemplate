using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Application.Repositories.KanbanBoundedContext;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.Infrastructure.Repositories.KanbanBoundedContext
{
    public class BoardRepository : IBoardRepository
    {
        private readonly FakeDbContext _dbContext;

        public BoardRepository(FakeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Board> FindAsync(Guid boardId, CancellationToken cancellationToken)
        {
            _dbContext.Boards.TryGetValue(boardId, out var board);
            return Task.FromResult(board);
        }

        public Task SaveAsync(Board board, CancellationToken cancellationToken)
        {
            _dbContext.Boards.AddOrUpdate(board.Id, board, (id, oldBoard) => board);
            return Task.CompletedTask;
        }
    }
}