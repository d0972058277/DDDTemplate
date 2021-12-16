using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using MediatR;
using Moq;
using Project.Application.Commands.KanbanBoundedContext.CreateWorkflow;
using Project.Domain.Aggregates.KanbanBoundedContext;
using Project.Domain.Events.KanbanBoundedContext;
using Project.Domain.Exceptions;
using Xunit;

namespace Project.UseCase.Test.Commands.KanbanBoundedContext.CreateWorkflow
{
    public class CreateWorkflowCommandTest
    {
        Mock<FakeWorkflowRepository> _mockWorkflowRepository;
        Mock<FakeEventMediator> _mockEventMediator;

        public CreateWorkflowCommandTest(Mock<FakeWorkflowRepository> mockWorkflowRepository, Mock<FakeEventMediator> mockEventMediator)
        {
            _mockWorkflowRepository = mockWorkflowRepository;
            _mockEventMediator = mockEventMediator;
        }

        FakeWorkflowRepository WorkflowRepository => _mockWorkflowRepository.Object;

        IEventMediator EventMediator => _mockEventMediator.Object;

        [Fact]
        public async Task CreateWorkflowCommand成功創建Workflow()
        {
            // Given
            var workflowId = Guid.NewGuid();
            var name = "workflowName";
            var boardId = Guid.NewGuid();

            var command = new CreateWorkflowCommand(workflowId, name, boardId);
            var handler = new CreateWorkflowCommandHandler(WorkflowRepository, EventMediator);
            var cancellationToken = default(CancellationToken);

            // When
            await handler.Handle(command, cancellationToken);

            // Then
            _mockWorkflowRepository.Verify(m => m.FindAsync(workflowId, cancellationToken), Times.Once());
            _mockWorkflowRepository.Verify(m => m.SaveAsync(It.IsAny<Workflow>(), cancellationToken), Times.Once());
            _mockEventMediator.Verify(m => m.PublishDomainEventAsync(It.Is<IDomainEvent>(e => e is WorkflowCreatedDomainEvent), cancellationToken), Times.Once());
        }

        [Fact]
        public async Task CreateWorkflowCommand不能重複創建相同的Workflow()
        {
            // Given
            var workflowId = Guid.NewGuid();
            var name = "workflowName";
            var boardId = Guid.NewGuid();
            var workflow = Workflow.Create(workflowId, name, boardId);
            workflow.ClearDomainEvents();
            WorkflowRepository.Init(workflow);

            var command = new CreateWorkflowCommand(workflowId, name, boardId);
            var handler = new CreateWorkflowCommandHandler(WorkflowRepository, EventMediator);
            var cancellationToken = default(CancellationToken);

            // When
            Func<Task<Unit>> func = () => handler.Handle(command, cancellationToken);

            // Then
            await func.Should().ThrowAsync<DomainException>();
        }
    }
}