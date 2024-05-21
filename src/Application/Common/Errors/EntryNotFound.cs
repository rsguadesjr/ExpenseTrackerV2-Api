using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Errors
{
    public class EntryNotFound : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;

        public string Message => "Item not found due to invalid id provided.";
    }
}
