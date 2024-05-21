using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Common.Interfaces.Persistence
{
    public interface IExpenseTrackerDbContext
    {
        public DbSet<ExpenseTracker.Domain.Entities.Transaction> Transactions { get; set; }
        public DbSet<TransactionTag> TransactionTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Domain.Entities.Account> Accounts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
