using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiController
    {
        private readonly ISender _mediator;
        public UsersController(ISender mediator)
        {
            _mediator = mediator;
        }

        //// GET api/users/user-info
        //[HttpGet]
        //[Route("user-info")]
        //[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetUserInfo()
        //{
        //    var result = await _mediator.Send(new GetUserQuery());
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result);
        //}
    }
}
