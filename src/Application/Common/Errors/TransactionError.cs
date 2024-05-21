using ExpenseTracker.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    public class TransactionError
    {
        public static readonly Error NotFound = new Error("Transaction.NotFound", "The requested resource was not found.", ErrorType.NotFound);
    }
}
