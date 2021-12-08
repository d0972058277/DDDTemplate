using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Application.Repositories.KanbanBoundedContext;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.Application.Queries.KanbanBoundedContext.ListBoards
{
    public class ListBoardsQueryHandler : IQueryHandler<ListBoardsQuery, IEnumerable<Board>>
    {
        private readonly IListBoardsQueryRepository _listBoardsQueryRepository;

        public ListBoardsQueryHandler(IListBoardsQueryRepository listBoardsQueryRepository)
        {
            _listBoardsQueryRepository = listBoardsQueryRepository;
        }

        public Task<IEnumerable<Board>> Handle(ListBoardsQuery request, CancellationToken cancellationToken)
        {
            return _listBoardsQueryRepository.ExecuteAsync(cancellationToken);
        }
    }
}