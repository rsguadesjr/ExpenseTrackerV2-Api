using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Transactions.Commands.CreateTransaction;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTracker.Application.Tests.Transactions.Commands.CreateTransaction
{
    public class UpdateTransactionCommandValidationTest : BaseApplicationTest
    {
        [Fact]
        public async Task Validation_Should_ReturnSuccessResult_WhenPropertiesAreValid()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var account = dbContext.Accounts.First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);
            var command = new CreateTransactionCommand
            {
                AccountId = account.Id,
                Amount = 100,
                CategoryId = Guid.NewGuid(),
                Description = "Transaction description",
                TransactionDate = DateTime.Now,
                Tags = new List<string> { "tag1", "tag2" }
            };

            // act
            var validator = new CreateTransactionCommandValidator(dbContext, requestContext.Object);
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
            var command = new CreateTransactionCommand
            {
                Amount = 0,
                CategoryId = Guid.Empty,
                Description = string.Join("", Enumerable.Repeat("a", 251).ToArray()),
                TransactionDate = DateTime.MinValue,
                Tags = new List<string>()
            };

            // act
            var validator = new CreateTransactionCommandValidator(dbContext, requestContext.Object);
            var validationResult = await validator.ValidateAsync(command);

            // assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateTransactionCommand.Amount));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateTransactionCommand.CategoryId));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateTransactionCommand.TransactionDate));
            validationResult.Errors.Should().Contain(x => x.PropertyName == nameof(CreateTransactionCommand.AccountId));
        }
    }
}
