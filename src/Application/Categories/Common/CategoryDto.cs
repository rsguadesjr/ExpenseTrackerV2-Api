using ExpenseTracker.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Categories.Common
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        //public KeyValueItem<Guid, string> User { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Order { get; set; }
        //public KeyValueItem<int, string> Source { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public KeyValueItem<Guid, string> CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public KeyValueItem<Guid, string> ModifiedBy { get; set; }
    }
}
