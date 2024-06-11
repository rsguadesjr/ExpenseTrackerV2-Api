using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("/error")]
    [AllowAnonymous]
    public class ErrorsController : ApiController
    {
        private readonly IHostEnvironment _env;
        public ErrorsController(IHostEnvironment env)
        {
            _env = env;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (_env.IsDevelopment())
            {
                return Problem(title: exception?.Message, detail: exception?.StackTrace);
            }

            return Problem(title: "Server Error", detail: "An unhandled error occurred.");
        }
    }
}
