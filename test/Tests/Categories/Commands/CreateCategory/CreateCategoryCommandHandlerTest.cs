using ExpenseTracker.Application.Categories.Commands.CreateCategory;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExpenseTracker.Application.Tests.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandlerTest : BaseApplicationTest
    {
        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenPropertiesAreValid()
        {

            var dbContext = await GetDbContext();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(Guid.NewGuid());

            // arrange
            var command = new CreateCategoryCommand
            {
                Name = "Category Name",
                Description = "Description",
                Order = 1
            };

            var handler = new CreateCategoryCommandHandler(dbContext, requestContext.Object);

            // act
            var result = await handler.Handle(command, default);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
