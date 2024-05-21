using ExpenseTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Domain.Entities
{
    public class Transaction : IAuditableEntity
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = null!;
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }
        public virtual ICollection<TransactionTag> TransactionTags { get; set; }

    }
}
