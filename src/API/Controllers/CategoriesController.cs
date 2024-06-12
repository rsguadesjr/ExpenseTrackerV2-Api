using ExpenseTracker.Application.Categories.Commands.CreateCategory;
using ExpenseTracker.Application.Categories.Commands.DeleteCategory;
using ExpenseTracker.Application.Categories.Commands.UpdateCategory;
using ExpenseTracker.Application.Categories.Queries.GetCategories;
using ExpenseTracker.Application.Categories.Queries.GetCategoryById;
using ExpenseTracker.Contracts.Category;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoriesController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public CategoriesController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // POST api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            var command = _mapper.Map<CreateCategoryCommand>(request);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, CreateCategoryRequest request)
        {
            var command = _mapper.Map<UpdateCategoryCommand>((id, request));
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // GET api/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var query = new GetCategoriesQuery();
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(result.Errors);
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var command = new DeleteCategoryCommand { Id = id };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return Problem(result.Errors);
        }
    }
}
