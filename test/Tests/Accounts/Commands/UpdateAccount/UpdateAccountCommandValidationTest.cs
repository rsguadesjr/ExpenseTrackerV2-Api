using ExpenseTracker.Application.Accounts.Commands.UpdateAccount;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTracker.Application.Tests.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommandValidationTest
    {
        // create test similar to UpdateTransactionCommandValidationTest
        [Fact]
        public void Validation_Should_Pass_WhenPropertiesAreValid()
        {
            // arrange
            var command = new UpdateAccountCommand
            {
                Id = Guid.NewGuid(),
                Name = "Account Name",
                Description = "Description",
                IsDefault = false
            };

            // act
            var validator = new UpdateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validation_Should_ReturnValidationErrors_WhenPropertiesAreInvalid()
        {
            // arrange
            var command = new UpdateAccountCommand
            {
                Id = Guid.Empty,
                Name = string.Empty,
                Description = string.Join("", Enumerable.Repeat("a", 101).ToArray()),
                IsDefault = false
            };

            // act
            var validator = new UpdateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateAccountCommand.Id));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateAccountCommand.Name));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateAccountCommand.Description));
        }

        [Fact]
        public void Validation_Should_ReturnValidationError_WhenAccountNameIsInvalid()
        {
            // arrange
            var command = new UpdateAccountCommand
            {
                Id = Guid.NewGuid(),
                Name = "Account Name#??",
                Description = string.Empty,
                IsDefault = false
            };

            // act
            var validator = new UpdateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateAccountCommand.Name));
        }

        [Fact]
        public void Validation_Should_ReturnValidationError_WhenAccountNameIsTooShort()
        {
            // arrange
            var command = new UpdateAccountCommand
            {
                Id = Guid.NewGuid(),
                Name = "Ac",
                Description = string.Empty,
                IsDefault = false
            };

            // act
            var validator = new UpdateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateAccountCommand.Name));
        }

        [Fact]
        public void Validation_Should_ReturnValidationErrors_WhenAccountNameIsTooLong()
        {
            // arrange
            var command = new UpdateAccountCommand
            {
                Id = Guid.NewGuid(),
                Name = string.Join("", Enumerable.Repeat("a", 21).ToArray()),
                Description = string.Empty,
                IsDefault = false
            };

            // act
            var validator = new UpdateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateAccountCommand.Name));
        }
    }
}
