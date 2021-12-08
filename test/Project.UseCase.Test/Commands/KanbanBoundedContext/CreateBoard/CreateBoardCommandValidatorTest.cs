using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Project.Application.Commands.KanbanBoundedContext.CreateBoard;
using Xunit;

namespace Project.UseCase.Test.Commands.KanbanBoundedContext.CreateBoard
{
    public class CreateBoardCommandValidatorTest
    {
        [Fact]
        public void CreateBoardCommandValidator驗證通過()
        {
            // Given
            var command = new CreateBoardCommand(Guid.NewGuid(), "name", Guid.NewGuid());
            var validator = new CreateBoardCommandValidator();

            // When
            var result = validator.Validate(command);

            // Then
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateBoardCommandValidator驗證BoardId不能為空()
        {
            // Given
            var command = new CreateBoardCommand(Guid.Empty, "name", Guid.NewGuid());
            var validator = new CreateBoardCommandValidator();

            // When
            var result = validator.Validate(command);

            // Then
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateBoardCommandValidator驗證Name不能是空字串()
        {
            // Given
            var command = new CreateBoardCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid());
            var validator = new CreateBoardCommandValidator();

            // When
            var result = validator.Validate(command);

            // Then
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateBoardCommandValidator驗證UserId不能為空()
        {
            // Given
            var command = new CreateBoardCommand(Guid.NewGuid(), "name", Guid.Empty);
            var validator = new CreateBoardCommandValidator();

            // When
            var result = validator.Validate(command);

            // Then
            result.IsValid.Should().BeFalse();
        }
    }
}