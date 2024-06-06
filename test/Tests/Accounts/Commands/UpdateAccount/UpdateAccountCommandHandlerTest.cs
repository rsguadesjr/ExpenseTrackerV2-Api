using ExpenseTracker.Application.Accounts.Commands.CreateAccount;
using ExpenseTracker.Application.Accounts.Commands.UpdateAccount;
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

namespace ExpenseTracker.Application.Tests.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommandHandlerTest : BaseApplicationTest
    {
        // create test for UpdateAccountCommand
        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenCommandParametersAreValid()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);

            var account = dbContext.Accounts.First();
            var command = new UpdateAccountCommand
            {
                Id = account.Id,
                Name = "Account Updated",
                Description = "Account Updated description",
                IsDefault = true
            };
            var handler = new UpdateAccountCommandHandler(dbContext, requestContext.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be("Account Updated");
            result.Value.Description.Should().Be("Account Updated description");
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenUpdatingDefaultAccountToNotDefault()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);

            var account = dbContext.Accounts.First();
            var command = new UpdateAccountCommand
            {
                Id = account.Id,
                Name = "Account Updated",
                Description = "Account Updated description",
                IsDefault = false
            };
            var handler = new UpdateAccountCommandHandler(dbContext, requestContext.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(AccountError.CurrentAccountMustRemainDefault);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenUpdatedAccountNameIsNotUnique()
        {
            // arrange
            var dbContext = await GetDbContext();
            var user = dbContext.Users.First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(user.Id);

            var account = dbContext.Accounts.First();
            var command = new UpdateAccountCommand
            {
                Id = account.Id,
                Name = "Account 2",
                Description = "Account 2 description",
                IsDefault = false
            };
            var handler = new UpdateAccountCommandHandler(dbContext, requestContext.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(AccountError.NameNotUnique);
        }
    }
}
