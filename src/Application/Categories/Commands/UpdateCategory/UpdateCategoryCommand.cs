﻿using ExpenseTracker.Application.Categories.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<Result<CategoryDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? Order { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryDto>>
    {
        private readonly IRequestContext _requestContext;
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IMapper _mapper;
        public UpdateCategoryCommandHandler(IRequestContext requestContext, IExpenseTrackerDbContext dbContext, IMapper mapper)
        {
            _requestContext = requestContext;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FindAsync(request.Id, cancellationToken);
            if (category == null)
            {
                return Result<CategoryDto>.Failure(CategoryError.NotFound);
            }


            var updatedCategory = _mapper.Map(request, category);
            updatedCategory.UserId = _requestContext.UserId;

            _dbContext.Categories.Update(updatedCategory);
            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);

            var result = await _dbContext.Categories
                .ProjectToType<CategoryDto>()
                .SingleOrDefaultAsync(x => x.Id == category.Id, cancellationToken);

            return Result<CategoryDto>.Success(result);
        }
    }
}