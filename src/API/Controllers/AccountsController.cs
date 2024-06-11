using ExpenseTracker.Application.Accounts.Commands.CreateAccount;
using ExpenseTracker.Application.Accounts.Commands.DeleteAccount;
using ExpenseTracker.Application.Accounts.Commands.UpdateAccount;
using ExpenseTracker.Application.Accounts.Queries.GetAccountById;
using ExpenseTracker.Application.Accounts.Queries.GetAccounts;
using ExpenseTracker.Contracts.Account;
using MapsterMapper;
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
        private readonly IMapper _mapper;
        public AccountsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
            var query = new GetAccountByIdQuery { Id = id };
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
