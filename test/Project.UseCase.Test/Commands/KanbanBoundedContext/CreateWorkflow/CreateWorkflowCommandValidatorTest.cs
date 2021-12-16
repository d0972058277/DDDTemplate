using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Project.Application.Commands.KanbanBoundedContext.CreateWorkflow;
using Xunit;

namespace Project.UseCase.Test.Commands.KanbanBoundedContext.CreateWorkflow
{
    public class CreateWorkflowCommandValidatorTest
    {
        [Fact]
        public void CreateWorkflowCommandValidator驗證通過()
        {
            // Given
            var command = new CreateWorkflowCommand(Guid.NewGuid(), "name", Guid.NewGuid());
            var validator = new CreateWorkflowCommandValidator();

            // When
            var result = validator.Validate(command);

            // Then
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateWorkflowCommandValidator驗證WorkflowId不能為空()
        {
            // Given
            var command = new CreateWorkflowCommand(Guid.Empty, "name", Guid.NewGuid());
            var validator = new CreateWorkflowCommandValidator();

            // When
            var result = validator.Validate(command);

            // Then
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateWorkflowCommandValidator驗證Name不能是空字串()
        {
            // Given
            var command = new CreateWorkflowCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid());
            var validator = new CreateWorkflowCommandValidator();

            // When
            var result = validator.Validate(command);

            // Then
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateWorkflowCommandValidator驗證BoardId不能為空()
        {
            // Given
            var command = new CreateWorkflowCommand(Guid.NewGuid(), "name", Guid.Empty);
            var validator = new CreateWorkflowCommandValidator();

            // When
            var result = validator.Validate(command);

            // Then
            result.IsValid.Should().BeFalse();
        }
    }
}