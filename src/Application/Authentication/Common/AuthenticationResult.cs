using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Authentication.Common
{
    public class AuthenticationResult
    {
        public string? AccessToken { get; set; }
        public bool IsEmailVerified { get; set; }
    }

}
