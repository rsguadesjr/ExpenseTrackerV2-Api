﻿using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Application.Transactions.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<Result<TransactionDto>>
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime TransactionDate { get; set; }
        public List<string> Tags { get; set; }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRequestContext _requestContext;

        public CreateTransactionCommandHandler(IExpenseTrackerDbContext dbContext, IMapper mapper, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _requestContext = requestContext;
        }
        public async Task<Result<TransactionDto>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            // validate AccountId if it belongs to the user
            var accountBelongsToUser = await _dbContext.Accounts
                                                        .AnyAsync(x => x.UserId == _requestContext.UserId && x.Id == request.AccountId, cancellationToken);
            if (!accountBelongsToUser)
            {
                return Result<TransactionDto>.Failure(AccountError.NotFound);
            }

            var transaction = new Domain.Entities.Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                Amount = request.Amount,
                Description = request.Description,
                CategoryId = request.CategoryId,
                TransactionDate = request.TransactionDate,
            };
            await _dbContext.Transactions.AddAsync(transaction, cancellationToken);
            await AddTags(request, transaction.Id, cancellationToken);

            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);

            var result = await _dbContext.Transactions
                .AsNoTracking()
                .ProjectToType<TransactionDto>()
                .SingleOrDefaultAsync(x => x.Id == transaction.Id, cancellationToken);

            return Result<TransactionDto>.Success(result!);
        }


        private async Task AddTags(CreateTransactionCommand request, Guid transactionId, CancellationToken cancellationToken)
        {
            if (request.Tags == null || request.Tags.Count == 0)
            {
                return;
            }

            // remove duplicates and trim tags from the request
            request.Tags = request.Tags.Select(x => x.Trim()).Distinct().ToList();

            // existing tags in database that are in the request
            var existingUserTags = await _dbContext.Tags
                .Where(x => x.UserId == _requestContext.UserId && request.Tags.Contains(x.Name))
                .ToListAsync();

            // add to transaction tags that are not yet in the transaction
            foreach (var requestTag in request.Tags)
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

                var transactionTag = new TransactionTag
                {
                    TransactionId = transactionId,
                    TagId = tag.Id
                };
                await _dbContext.TransactionTags.AddAsync(transactionTag, cancellationToken);
            }
        }
    }
}
