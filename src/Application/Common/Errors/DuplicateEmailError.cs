using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    public class DuplicateEmailError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string Message => "User with the same email already registered";
    }
}
