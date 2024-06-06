using ExpenseTracker.Application.Categories.Commands.UpdateCategory;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTracker.Application.Tests.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandlerTest : BaseApplicationTest
    {
        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenGivenValidRequest()
        {
            // Arrange
            var dbContext = await GetDbContext();
            var user = GetDummyUsers().First();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(dbContext.Users.First().Id);

            var categoryId = dbContext.Categories.AsNoTracking().Select(x => x.Id).First();
            var request = new UpdateCategoryCommand
            {
                Id = categoryId,
                Name = "Updated Category",
                Description = "Updated Category Description",
                IsActive = true,
                Order = 1
            };

            var mapper = new Mock<IMapper>();
            mapper.Setup(mapper => mapper.Map(It.IsAny<UpdateCategoryCommand>(), It.IsAny<Domain.Entities.Category>())).Returns(new Domain.Entities.Category
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                Order = request.Order
            });

            // Act
            var handler = new UpdateCategoryCommandHandler(dbContext, requestContext.Object, mapper.Object);
            var result = await handler.Handle(request, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenCategoryDoesNotBelongToTheUser()
        {
            // Arrange
            var dbContext = await GetDbContext();
            var requestContext = new Mock<IRequestContext>();
            requestContext.Setup(x => x.UserId).Returns(dbContext.Users.First().Id);
            var mapper = new Mock<IMapper>();
            var category = dbContext.Categories.First();

            var handler = new UpdateCategoryCommandHandler(dbContext, requestContext.Object, mapper.Object);
            var request = new UpdateCategoryCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Category",
                Description = "Updated Category Description",
                IsActive = true,
                Order = 1
            };

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(CategoryError.NotFound);
        }
    }
}
