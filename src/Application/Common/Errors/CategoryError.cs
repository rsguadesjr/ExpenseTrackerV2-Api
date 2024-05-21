using ExpenseTracker.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    public class CategoryError
    {
        public static readonly Error NotFound = new Error("Category.NotFound", "The requested resource was not found.", ErrorType.NotFound);
    }
}
