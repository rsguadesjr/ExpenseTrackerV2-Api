using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    internal class InvalidTokenError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public string Message => "Invalid token";
    }
}
