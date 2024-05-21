using ExpenseTracker.Application.Account.Commands.CreateAccount;
using ExpenseTracker.Application.Account.Commands.DeleteAccount;
using ExpenseTracker.Application.Account.Commands.UpdateAccount;
using ExpenseTracker.Application.Account.Queries.GetAccount;
using ExpenseTracker.Application.Account.Queries.GetAccounts;
using ExpenseTracker.Contracts.Account;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ApiController
    {
        private readonly ISender _mediator;
        public AccountsController(ISender mediator)
        {
            _mediator = mediator;
        }

        // GET api/accounts
        [HttpGet]
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
        public async Task<IActionResult> GetAccount([FromRoute] Guid id)
        {
            var query = new GetAccountQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
        {
            var command = new CreateAccountCommand { Name = request.Name, Description = request.Description, IsDefault = request.IsDefault };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // PUT api/accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount([FromRoute] Guid id, CreateAccountRequest request)
        {
            var command = new UpdateAccountCommand { Id = id, Name = request.Name, Description = request.Description, IsDefault = request.IsDefault };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        //DELETE api/accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id, [FromQuery] bool? forceDelete)
        {
            var command = new DeleteAccountCommand { Id = id, ForceDelete = forceDelete ?? false };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }
    }
}
