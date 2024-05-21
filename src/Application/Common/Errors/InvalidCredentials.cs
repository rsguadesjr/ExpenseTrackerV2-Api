using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    public class InvalidCredentials : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string Message => "Invalid email or password.";
    }
}
