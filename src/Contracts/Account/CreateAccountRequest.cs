using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.Account
{
    public record CreateAccountRequest
    {
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public required bool IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }
}
