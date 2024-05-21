using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Persistence
{
    public class ExpenseTrackerDbContext : DbContext, IExpenseTrackerDbContext
    {
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TransactionTag> TransactionTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var dateTime = DateTime.UtcNow;
            OnBeforeSaveChanges(dateTime, userId);
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.ClrType.Name);
            }
        }

        private void OnBeforeSaveChanges(DateTime? dateTime, Guid? userId)
        {
            ChangeTracker.DetectChanges();

            var auditableEntries = ChangeTracker.Entries<IAuditableEntity>().Where(x => x.Entity is not null && x.State != EntityState.Unchanged).ToList();
            foreach (var entry in auditableEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = dateTime;
                        entry.Entity.CreatedById = userId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = dateTime;
                        entry.Entity.ModifiedById = userId;
                        break;
                }
            }
        }
    }
}
