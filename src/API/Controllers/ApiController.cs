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

            if (errors.All(error => error.ErrorType == ErrorType.Validation))
            {
                var modelDictionary = new ModelStateDictionary();
                foreach (var err in errors)
                {
                    modelDictionary.AddModelError(err.Code, err.Description);
                }

                return ValidationProblem(modelDictionary);
            }

            var error = errors[0];
            var statusCode = GetStatusCode(error.ErrorType);
            var title = GetTitle(error.ErrorType);

            return Problem(title: title, detail: error.Description, statusCode: statusCode, extensions: new Dictionary<string, object?>
                        {
                            { "errorCode", error.Code }
                        });
        }

        private static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
        }


        private static string GetTitle(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Unauthorized => "Unauthorized",
                ErrorType.Conflict => "Conflict",
                _ => "Server Error"
            };
        }
        private ObjectResult Problem(string? detail = null,
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
