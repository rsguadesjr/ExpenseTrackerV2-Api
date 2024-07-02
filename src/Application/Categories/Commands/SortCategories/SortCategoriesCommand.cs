using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Categories.Commands.SortCategories
{
    public class SortCategoriesCommand : IRequest<Result<string>>
    {
        public List<SortCategoryRequest> SortRequests { get; set; } = [];
    }

    public class SortCategoriesCommandHandler : IRequestHandler<SortCategoriesCommand, Result<string>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IRequestContext _requestContext;
        public SortCategoriesCommandHandler(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }
        public async Task<Result<string>> Handle(SortCategoriesCommand request, CancellationToken cancellationToken)
        { 
        
            var categories = await _dbContext.Categories
                                                .Where(x => x.UserId == _requestContext.UserId)
                                                .ToListAsync(cancellationToken);

            foreach (var sortRequest in request.SortRequests)
            {
                var category = categories.SingleOrDefault(x => x.Id == sortRequest.Id);
                if (category == null)
                {
                    return Result<string>.Failure(CategoryError.NotFound);
                }
                else
                {
                    category.Order = sortRequest.Order;
                }
            }

            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);


            return Result<string>.Success("");
        }
    }
}
