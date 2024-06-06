using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Transactions.Commands.CreateTransaction;
using FluentAssertions;
using MapsterMapper;
using Moq;
using Xunit;

namespace ExpenseTracker.Application.Tests.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandlerTest : BaseApplicationTest
    {

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenCommandParametersAreValid()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var account = dbContext.Accounts.First();

            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);

            var mapper = new Mock<IMapper>();

            var handler = new CreateTransactionCommandHandler(dbContext, mapper.Object, requestContext.Object);
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
            var result = await handler.Handle(command, default);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenAccountIdIsInvalid()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();

            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);

            var mapper = new Mock<IMapper>();

            var handler = new CreateTransactionCommandHandler(dbContext, mapper.Object, requestContext.Object);
            var command = new CreateTransactionCommand
            {
                AccountId = Guid.NewGuid(),
                Amount = 100,
                CategoryId = Guid.NewGuid(),
                Description = "Transaction description",
                TransactionDate = DateTime.Now,
                Tags = new List<string> { "tag1", "tag2" }
            };

            // act
            var result = await handler.Handle(command, default);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(AccountError.NotFound);
        }

    }
}
