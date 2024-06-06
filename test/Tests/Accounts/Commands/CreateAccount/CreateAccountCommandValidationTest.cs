using ExpenseTracker.Application.Accounts.Commands.CreateAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;
using FluentValidation.Results;

namespace ExpenseTracker.Application.Tests.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidationTest
    {
        [Fact]
        public void Validation_Should_Pass_WhenPropertiesAreValid()
        {
            // arrange
            var command = new CreateAccountCommand
            {
                Name = "Account Name",
                Description = "Description",
                IsDefault = false
            };

            // act
            var validator = new CreateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validation_Should_ReturnValidationErrors_WhenPropertiesAreInvalid()
        {
            // arrange
            var command = new CreateAccountCommand
            {
                Name = string.Empty,
                Description = string.Join("", Enumerable.Repeat("a", 101).ToArray()),
                IsDefault = false
            };

            // act
            var validator = new CreateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateAccountCommand.Name));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateAccountCommand.Description));
        }

        [Fact]
        public void Validation_Should_ReturnValidationError_WhenAccountNameContainsInvalidCharacters()
        {
            // arrange
            var command = new CreateAccountCommand
            {
                Name = "Account Name#?",
                Description = "Description",
                IsDefault = false
            };

            // act
            var validator = new CreateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateAccountCommand.Name));
        }

        [Fact]
        public void Validation_Should_ReturnValidationError_WhenAccountNameIsTooShort()
        {
            // arrange
            var command = new CreateAccountCommand
            {
                Name = "Ac",
                Description = "Description",
                IsDefault = false
            };

            // act
            var validator = new CreateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateAccountCommand.Name));
        }

        [Fact]
        public void WhenAccountNameIsTooLong_ShowValidationError()
        {
            // arrange
            var command = new CreateAccountCommand
            {
                Name = string.Join("", Enumerable.Repeat("a", 21).ToArray()),
                Description = "Description",
                IsDefault = false
            };

            // act
            var validator = new CreateAccountCommandValidator();
            var validationResult = validator.Validate(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateAccountCommand.Name));
        }
    }
}
