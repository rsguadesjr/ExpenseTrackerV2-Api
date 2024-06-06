using ExpenseTracker.Application.Categories.Commands.CreateCategory;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTracker.Application.Tests.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidationTest
    {
        [Fact]
        public void Validation_Should_Pass_WhenPropertiesAreValid()
        {
            // arrange
            var command = new CreateCategoryCommand
            {
                Name = "Category Name",
                Description = "Description",
                IsActive = true,
                Order = 1
            };

            // act
            var validator = new CreateCategoryCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validation_Should_ReturnValidationErrors_WhenPropertiesAreInvalid()
        {
            // arrange
            var command = new CreateCategoryCommand
            {
                Name = string.Empty,
                Description = string.Join("", Enumerable.Repeat("a", 51).ToArray()),
                IsActive = true,
                Order = -1
            };

            // act
            var validator = new CreateCategoryCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateCategoryCommand.Name));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateCategoryCommand.Description));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateCategoryCommand.Order));
        }
    }
}
