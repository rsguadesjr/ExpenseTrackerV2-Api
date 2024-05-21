using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.Account
{
    public record DeleteAccountRequest
    {
        public bool? HardDelete { get; set; }
    }
}
