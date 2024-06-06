using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Domain.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.Count == 0)
            {
                return Problem();
            }

            var errorObjects = new List<object>();

            if (errors.All(error => error.ErrorType == ErrorType.Validation))
            {
                errorObjects = errors.Select(e => new
                {
                    Field = e.Code,
                    e.Description,
                }).ToList<object>();
            }
            else
            {
                errorObjects = errors.Select(e => new
                {
                    e.Code,
                    e.Description,
                }).ToList<object>();
            }

            var error = errors[0];
            var statusCode = GetStatusCode(error.ErrorType);

            return Problem(
                statusCode: (int)statusCode,
                title: error.ErrorType.ToString(),
                extensions: new Dictionary<string, object?>
                {
                    { "errors", errorObjects }
                });
      
        }

        private HttpStatusCode GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => HttpStatusCode.BadRequest,
                ErrorType.NotFound => HttpStatusCode.NotFound,
                ErrorType.Forbidden => HttpStatusCode.Forbidden,
                ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
                ErrorType.Conflict => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };
        }
        protected ObjectResult Problem(string? detail = null,
                                     string? instance = null,
                                     int? statusCode = null,
                                     string? title = null,
                                     string? type = null,
                                     IDictionary<string, object?>? extensions = null)
        {
            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: statusCode ?? 500,
                title: title,
                type: type,
                detail: detail,
                instance: instance);

            if (extensions is not null)
            {
                foreach (var extension in extensions)
                {
                    problemDetails.Extensions.Add(extension);
                }
            }

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }
    }
}
