using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.Infrastructure
{
    public class FakeDbContext
    {
        public ConcurrentDictionary<Guid, Board> Boards { get; } = new ConcurrentDictionary<Guid, Board>();
    }
}