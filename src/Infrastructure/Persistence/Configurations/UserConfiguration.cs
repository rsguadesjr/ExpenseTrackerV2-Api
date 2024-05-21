using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.IdentityId).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Id).HasDefaultValueSql("newid()");
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.Avatar).HasMaxLength(250);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.IdentityId).IsUnique();

        }
    }
}
