using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations
{
    public class TransactionTagConfiguration : IEntityTypeConfiguration<TransactionTag>
    {
        public void Configure(EntityTypeBuilder<TransactionTag> builder)
        {
            // configure composite key
            builder.HasKey(x => new { x.TransactionId, x.TagId });

            //builder.HasOne(x => x.Transaction).WithMany(x => x.TransactionTags).HasForeignKey(x => x.TransactionId).OnDelete(DeleteBehavior.Clie);
            builder.HasOne(x => x.Tag).WithMany(x => x.TransactionTags).HasForeignKey(x => x.TagId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
