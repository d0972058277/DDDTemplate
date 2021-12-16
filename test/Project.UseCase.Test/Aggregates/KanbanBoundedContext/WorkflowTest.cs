using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Project.Domain.Aggregates.KanbanBoundedContext;
using Project.Domain.Events.KanbanBoundedContext;
using Xunit;

namespace Project.UseCase.Test.Aggregates.KanbanBoundedContext
{
    public class WorkflowTest
    {
        [Fact]
        public void Create()
        {
            // Given
            var workflowId = Guid.NewGuid();
            var name = "workflowName";
            var boardId = Guid.NewGuid();

            // When
            Workflow workflow = Workflow.Create(workflowId, name, boardId);

            // Then
            workflow.Id.Should().Be(workflowId);
            workflow.Name.Should().Be(name);
            workflow.BoardId.Should().Be(boardId);
            workflow.DomainEvents.Should().NotBeEmpty();
            workflow.DomainEvents.Any(e => e is WorkflowCreatedDomainEvent).Should().BeTrue();
            workflow.DomainEvents.Single(e => e is WorkflowCreatedDomainEvent).As<WorkflowCreatedDomainEvent>().WorkflowId.Should().Be(workflowId);
        }
    }
}