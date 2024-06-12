using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Transactions.Common
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public KeyValueItem<Guid, string> Category { get; set; }
        public DateTime TransactionDate { get; set; }
        //public KeyValueItem<Guid, string> User { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public KeyValueItem<Guid, string> CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public KeyValueItem<Guid, string> ModifiedBy { get; set; }
        public List<string> Tags { get; set; }
    }
}
