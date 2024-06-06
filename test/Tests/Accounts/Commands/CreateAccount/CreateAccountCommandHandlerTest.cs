using ExpenseTracker.Application.Accounts.Commands.CreateAccount;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTracker.Application.Tests.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandlerTest : BaseApplicationTest
    {
        // create tests for CreateAccountCommand
        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenCommandParametersAreValid()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);
            var command = new CreateAccountCommand
            {
                Name = "Account New",
                Description = "Account New description",
                IsDefault = false
            };
            var handler = new CreateAccountCommandHandler(dbContext, requestContext.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        // should return error if account name is not unique
        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenAccountNameIsNotUnique()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);
            var command = new CreateAccountCommand
            {
                Name = "Account 1",
                Description = "Account 1 description",
                IsDefault = false
            };
            var handler = new CreateAccountCommandHandler(dbContext, requestContext.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(AccountError.NameNotUnique);
        }

    }
}
