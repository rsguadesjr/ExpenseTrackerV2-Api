using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Transactions.Commands.CreateTransaction;
using ExpenseTracker.Application.Transactions.Commands.UpdateTransaction;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTracker.Application.Tests.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandValidationTest : BaseApplicationTest
    {
        [Fact]
        public async Task Validation_Should_ReturnSuccessResult_WhenParametersAreValid()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var existingAccount = dbContext.Accounts.First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);

            var command = new UpdateTransactionCommand
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                CategoryId = Guid.NewGuid(),
                Description = "Transaction description",
                TransactionDate = DateTime.Now,
                Tags = new List<string> { "tag1", "tag2" }
            };

            // act
            var validator = new UpdateTransactionCommandValidator(dbContext, requestContext.Object);
            var validationResult = await validator.ValidateAsync(command);

            // assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Validation_Should_ReturnFailureResult_WhenPropertiesAreInvalid()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);
            var longDescription = string.Join("", Enumerable.Repeat("a", 251).ToArray());

            var command = new UpdateTransactionCommand
            {
                Id = Guid.Empty,
                Amount = 0,
                CategoryId = Guid.Empty,
                Description = longDescription,
                TransactionDate = DateTime.MinValue,
                Tags = new List<string>()
            };

            // act
            var validator = new UpdateTransactionCommandValidator(dbContext, requestContext.Object);
            var validationResult = await validator.ValidateAsync(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateTransactionCommand.Amount));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateTransactionCommand.CategoryId));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateTransactionCommand.TransactionDate));
        }

    }
}
