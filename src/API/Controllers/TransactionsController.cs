using ExpenseTracker.Application.Transactions.Commands.CreateTransaction;
using ExpenseTracker.Application.Transactions.Commands.DeleteTransaction;
using ExpenseTracker.Application.Transactions.Commands.UpdateTransaction;
using ExpenseTracker.Application.Transactions.Queries.GetTransactionById;
using ExpenseTracker.Application.Transactions.Queries.GetTransactionsByMonthAndYear;
using ExpenseTracker.Contracts.Transaction;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TransactionsController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public TransactionsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(CreateTransactionRequest request)
        {
            var command = _mapper.Map<CreateTransactionCommand>(request);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction([FromRoute] Guid id, CreateTransactionRequest request)
        {
            var command = _mapper.Map<UpdateTransactionCommand>((id, request));
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // GET api/transactions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction([FromRoute] Guid id)
        {
            var query = new GetTransactionByIdQuery { TransactionId = id };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // GET api/transactions
        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] int year, [FromQuery] int? month)
        {
            var query = new GetTransactionsByMonthAndYearQuery { Year = year, Month = month };
            var result = await _mediator.Send(query);


            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // DELETE api/transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            var command = new DeleteTransactionCommand { TransactionId = id };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }
    }
}
