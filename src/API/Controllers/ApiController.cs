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
            var statusCode = error.ErrorType switch
            {
                ErrorType.Validation => HttpStatusCode.BadRequest,
                ErrorType.NotFound => HttpStatusCode.NotFound,
                ErrorType.Forbidden => HttpStatusCode.Forbidden,
                ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
                ErrorType.Conflict => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

            return Problem(statusCode: (int)statusCode, title: error.Description);
        }
    }
}
