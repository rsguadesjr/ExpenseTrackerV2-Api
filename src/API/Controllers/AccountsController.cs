using ExpenseTracker.Application.Accounts.Commands.CreateAccount;
using ExpenseTracker.Application.Accounts.Commands.DeleteAccount;
using ExpenseTracker.Application.Accounts.Commands.UpdateAccount;
using ExpenseTracker.Application.Accounts.Common;
using ExpenseTracker.Application.Accounts.Queries.GetAccountById;
using ExpenseTracker.Application.Accounts.Queries.GetAccounts;
using ExpenseTracker.Contracts.Account;
using ExpenseTracker.Contracts.Common;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public AccountsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // POST api/accounts
        [HttpPost]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status409Conflict, MediaTypeNames.Application.ProblemJson)]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
        {
            var command = _mapper.Map<CreateAccountCommand>(request);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // PUT api/accounts/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status409Conflict, MediaTypeNames.Application.ProblemJson)]
        public async Task<IActionResult> UpdateAccount([FromRoute] Guid id, CreateAccountRequest request)
        {
            var command = _mapper.Map<UpdateAccountCommand>((id, request));
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }


        // GET api/accounts
        [HttpGet]
        [ProducesResponseType(typeof(List<AccountDto>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAccounts()
        {
            var query = new GetAccountsQuery();
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // GET api/accounts/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
        public async Task<IActionResult> GetAccount([FromRoute] Guid id)
        {
            var query = new GetAccountByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        //DELETE api/accounts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id, [FromQuery] bool? forceDelete)
        {
            var command = new DeleteAccountCommand { Id = id, ForceDelete = forceDelete ?? false };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return Problem(result.Errors);
        }
    }
}
