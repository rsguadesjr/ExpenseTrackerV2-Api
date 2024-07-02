using ExpenseTracker.Application.Categories.Commands.CreateCategory;
using ExpenseTracker.Application.Categories.Commands.DeleteCategory;
using ExpenseTracker.Application.Categories.Commands.SortCategories;
using ExpenseTracker.Application.Categories.Commands.UpdateCategory;
using ExpenseTracker.Application.Categories.Common;
using ExpenseTracker.Application.Categories.Queries.GetCategories;
using ExpenseTracker.Application.Categories.Queries.GetCategoryById;
using ExpenseTracker.Contracts.Category;
using ExpenseTracker.Contracts.Common;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

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
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
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
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
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
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
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
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
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



        // PUT api/categories/sorted-categories
        [HttpPost("sorted-categories")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
        public async Task<IActionResult> SortCategories([FromBody] List<Contracts.Category.SortCategoryRequest> sortCategories)
        {
            var mapped = _mapper.Map<List<Application.Categories.Commands.SortCategories.SortCategoryRequest>>(sortCategories);
            var command = new SortCategoriesCommand { SortRequests = new List<Application.Categories.Commands.SortCategories.SortCategoryRequest>(mapped) };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return Problem(result.Errors);
        }
    }
}
