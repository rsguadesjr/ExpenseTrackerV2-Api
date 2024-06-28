using ExpenseTracker.API.Controllers;
using ExpenseTracker.Application.Authentication.Commands.LoginWithToken;
using ExpenseTracker.Application.Authentication.Commands.Register;
using ExpenseTracker.Application.Authentication.Common;
using ExpenseTracker.Application.Authentication.Queries.LoginWithEmailAndPassword;
using ExpenseTracker.Contracts.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;


[Route("auth")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(result.Errors);

    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginWithEmailAndPassword(LoginRequest request)
    {
        var query = _mapper.Map<LoginWithEmailAndPasswordQuery>(request);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(result.Errors);
    }

    [HttpPost("token/login")]
    public async Task<IActionResult> LoginWithToken(SocialLogin request)
    {
        var query = _mapper.Map<LoginWithTokenCommand>(request);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(result.Errors);
    }
}