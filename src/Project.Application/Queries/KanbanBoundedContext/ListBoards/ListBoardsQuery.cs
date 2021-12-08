using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.Application.Queries.KanbanBoundedContext.ListBoards
{
    public class ListBoardsQuery : IQuery<IEnumerable<Board>> { }
}