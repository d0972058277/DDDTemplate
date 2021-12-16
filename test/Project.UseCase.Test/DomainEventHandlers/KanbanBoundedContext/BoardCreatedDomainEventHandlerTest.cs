using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Moq;
using Project.Application.Commands.KanbanBoundedContext.CreateWorkflow;
using Project.Application.DomainEventHandlers.KanbanBoundedContext;
using Project.Domain.Events.KanbanBoundedContext;
using Xunit;

namespace Project.UseCase.Test.DomainEventHandlers.KanbanBoundedContext
{
    public class BoardCreatedDomainEventHandlerTest
    {
        Mock<FakeEventMediator> _mockEventMediator;

        public BoardCreatedDomainEventHandlerTest(Mock<FakeEventMediator> mockEventMediator)
        {
            _mockEventMediator = mockEventMediator;
        }

        IEventMediator EventMediator => _mockEventMediator.Object;

        [Fact]
        public async Task Create_a_default_workflow_after_creating_the_board()
        {
            // Given
            var boardId = Guid.NewGuid();
            var domainEvent = new BoardCreatedDomainEvent(boardId);
            var handler = new BoardCreatedDomainEventHandler(EventMediator);
            var cancellationToken = default(CancellationToken);

            // When
            await handler.Handle(domainEvent, cancellationToken);

            // Then
            _mockEventMediator.Verify(m => m.PublishCommandAsync(It.IsAny<CreateWorkflowCommand>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}