using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.Common
{
    /// <summary>
    /// Represents a problem details object with additional error code.
    /// Use only in the ProducesResponseType attribute for documentation purposes.
    /// </summary>
    public class ApiProblemDetails
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public string TraceId { get; set; }
        public string ErrorCode { get; set; }
    }
}
