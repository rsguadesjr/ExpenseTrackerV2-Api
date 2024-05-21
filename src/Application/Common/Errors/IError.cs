using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    public interface IError
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
    }
}
