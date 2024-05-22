using ExpenseTracker.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    public class AccountError
    {
        public static readonly Error NoDefaultAccount = new("Account.NoDefaultAccount", "One account needs to be a default account", ErrorType.Conflict);
        public static readonly Error AtLeastOneAccountIsActive = new("Account.AtLeastOneAccountIsActive", "At least one account needs to be active", ErrorType.Conflict);
        public static readonly Error NotFound = new Error("Account.NotFound", "The requested resource was not found.", ErrorType.NotFound);
        public static readonly Error NameNotUnique = new Error("Account.NameNotUnique", "The account name must be unique", ErrorType.Conflict);
        public static readonly Error AccountHasTransactions = new Error("Account.AccountHasTransactions", "The account still have existing transactions.", ErrorType.Conflict);
        public static readonly Error CurrentAccountMustRemainDefault = new Error("Account.CurrentAccountMustRemainDefault", "Changing of status to not default is not allowed. Consider changing other account to default instead.", ErrorType.Conflict);
    }
}
