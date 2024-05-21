using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.Transaction
{

    public record CreateTransactionRequest
    {
        [Required]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        public Guid? AccountId { get; set; }
        public List<string> Tags { get; set; }
    }
}
