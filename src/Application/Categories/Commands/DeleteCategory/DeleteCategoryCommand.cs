using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using MediatR;

namespace ExpenseTracker.Application.Categories.Commands.DeleteCategory
{
    // generate DeleteCategoryCommand class with public int Id and implement IRequest<Result<string>>
    public class DeleteCategoryCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }
    }

    // generate DeleteCategoryCommandHandler class with IExpenseTrackerDbContext and IRequestContext as constructor parameters
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<string>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IRequestContext _requestContext;
        public DeleteCategoryCommandHandler(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }
        public async Task<Result<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FindAsync(request.Id);
            if (category == null)
            {
                return Result<string>.Failure(TransactionError.NotFound);
            }
            else
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync(_requestContext.UserId);
                return Result<string>.Success("");
            }
        }
    }
}
