using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.Category
{
    public record CreateCategoryRequest
    {
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? Order { get; set; }
    }
}
