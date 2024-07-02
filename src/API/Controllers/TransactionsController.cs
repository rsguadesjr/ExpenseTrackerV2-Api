using ExpenseTracker.Application.Transactions.Commands.Common;
using ExpenseTracker.Application.Transactions.Commands.CreateTransaction;
using ExpenseTracker.Application.Transactions.Commands.DeleteTransaction;
using ExpenseTracker.Application.Transactions.Commands.UpdateTransaction;
using ExpenseTracker.Application.Transactions.Queries.GetTransactionById;
using ExpenseTracker.Application.Transactions.Queries.GetTransactions;
using ExpenseTracker.Contracts.Common;
using ExpenseTracker.Contracts.Transaction;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

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
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
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
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
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
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
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
        [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
        public async Task<IActionResult> GetTransactions([FromQuery] int year,
                                                         [FromQuery] int? month,
                                                         [FromQuery] Guid? accountId,
                                                         [FromQuery] int timezoneOffset)
        {
            var query = new GetTransactionsQuery { Year = year, Month = month, AccountId = accountId, TimezoneOffset =  timezoneOffset};
            var result = await _mediator.Send(query);


            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // DELETE api/transactions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            var command = new DeleteTransactionCommand { TransactionId = id };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return Problem(result.Errors);
        }
    }
}
