using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Application.Transactions.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommand : IRequest<Result<TransactionDto>>
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime TransactionDate { get; set; }
        public List<string> Tags { get; set; }
    }

    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Result<TransactionDto>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRequestContext _requestContext;

        public UpdateTransactionCommandHandler(IExpenseTrackerDbContext dbContext, IMapper mapper, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _requestContext = requestContext;
        }

        public async Task<Result<TransactionDto>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {

            var transaction = await _dbContext.Transactions
                                        .Include(x => x.TransactionTags).ThenInclude(x => x.Tag)        
                                        .SingleOrDefaultAsync(x => x.Account.UserId == _requestContext.UserId && x.Id == request.Id, cancellationToken);
            if (transaction == null)
            {
                return Result<TransactionDto>.Failure(TransactionError.NotFound);
            }

            request.Adapt(transaction);

            await AddTags(request, transaction, cancellationToken);

            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);

            var result = await _dbContext.Transactions
                .AsNoTracking()
                .ProjectToType<TransactionDto>()
                .SingleOrDefaultAsync(x => x.Id == transaction.Id, cancellationToken);

            return Result<TransactionDto>.Success(result!);
        }

        private async Task AddTags(UpdateTransactionCommand request, Domain.Entities.Transaction transaction, CancellationToken cancellationToken)
        {
            if (request.Tags == null || request.Tags.Count == 0)
            {
                return;
            }
            // distinct tags and trim them
            request.Tags = request.Tags.Select(x => x.Trim()).Distinct().ToList();


            // get existing tags for the current user that are in the request
            var existingUserTags = await _dbContext.Tags
                                    .Where(x => x.UserId == _requestContext.UserId && request.Tags.Contains(x.Name))
                                    .AsNoTracking()
                                    .ToListAsync();

            // remove transaction tags that are not in the request
            foreach (var transactionTag in transaction.TransactionTags)
            {
                if (!request.Tags.Contains(transactionTag.Tag.Name))
                {
                    _dbContext.TransactionTags.Remove(transactionTag);
                }
            }

            // add to transaction tags that are not yet in the transaction
            foreach(var requestTag in request.Tags)
            {
                var tag = existingUserTags.FirstOrDefault(x => x.Name == requestTag);
                if (tag == null)
                {
                    tag = new Tag
                    {
                        Id = Guid.NewGuid(),
                        Name = requestTag,
                        UserId = _requestContext.UserId
                    };
                    await _dbContext.Tags.AddAsync(tag, cancellationToken);
                }

                var tr = transaction.TransactionTags.FirstOrDefault(x => x.TagId == tag.Id);
                if (tr == null)
                {
                    var transactionTag = new TransactionTag
                    {
                        TransactionId = transaction.Id,
                        TagId = tag.Id
                    };
                    await _dbContext.TransactionTags.AddAsync(transactionTag, cancellationToken);
                }
            }
        }
    }
}
