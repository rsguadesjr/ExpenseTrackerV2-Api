using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Common.Interfaces.Authentication
{
    public interface IRequestContext
    {
        public Guid UserId { get; }
    }
}
