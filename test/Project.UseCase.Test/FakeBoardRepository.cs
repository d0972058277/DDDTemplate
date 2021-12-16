using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Application.Repositories.KanbanBoundedContext;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.UseCase.Test
{
    public abstract class FakeBoardRepository : IBoardRepository
    {
        public Dictionary<Guid, Board> DataSet = new Dictionary<Guid, Board>();

        public abstract Task<Board> FindAsync(Guid boardId, CancellationToken cancellationToken);
        public abstract Task SaveAsync(Board board, CancellationToken cancellationToken);

        public void Init(Board board)
        {
            DataSet.TryAdd(board.Id, board);
        }
    }
}