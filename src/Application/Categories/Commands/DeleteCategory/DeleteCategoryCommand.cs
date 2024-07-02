using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Categories.Commands.DeleteCategory
{
    // generate DeleteCategoryCommand class with public int Id and implement IRequest<Result<string>>
    public class DeleteCategoryCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
    }

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
            // TODO: update db context to delete cascade null
            var category = await _dbContext.Categories.SingleOrDefaultAsync(x => x.UserId == _requestContext.UserId && x.Id == request.Id, cancellationToken);
            if (category == null)
            {
                return Result<string>.Failure(TransactionError.NotFound);
            }
            else
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);
                return Result<string>.Success("");
            }
        }
    }
}
