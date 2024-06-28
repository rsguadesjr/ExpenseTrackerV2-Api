using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Domain.Models.Common.Authentication
{
    public class VerifyTokenResult
    {
        public string IdentityId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool EmailVerified { get; set; }
    }
}
