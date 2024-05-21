﻿using ExpenseTracker.Application.Categories.Common;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Application.Transactions.Commands.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Result<CategoryDto>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? Order { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
    {
        private readonly IRequestContext _requestContext;
        private readonly IExpenseTrackerDbContext _dbContext;
        public CreateCategoryCommandHandler(IRequestContext requestContext, IExpenseTrackerDbContext dbContext)
        {
            _requestContext = requestContext;
            _dbContext = dbContext;
        }
        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive ?? true,
                Order = request.Order,
                UserId = _requestContext.UserId
            };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);

            var result = await _dbContext.Categories.Where(x => x.Id == category.Id)
                .ProjectToType<CategoryDto>()
                .SingleOrDefaultAsync(cancellationToken);

            return Result<CategoryDto>.Success(result);
        }
    }
}
