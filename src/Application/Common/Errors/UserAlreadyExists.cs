using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    public class UserAlreadyExists : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string Message => "User with the same email already exists.";
    }
}
