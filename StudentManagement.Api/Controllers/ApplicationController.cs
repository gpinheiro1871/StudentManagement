using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace StudentManagement.Api.Controllers;

[ApiController]
public abstract class ApplicationController : BaseController
{
    private readonly IMediator _mediator;

    protected ApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<IActionResult> FromQuery<T>(IRequest<T> query)
    {
        T result = await _mediator.Send(query);

        return Ok(result);
    }    
    
    protected async Task<IActionResult> FromQuery<T>(string resourceName, 
        IRequest<Result<T, Error>> query)
    {
        Result<T, Error> result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return Error(resourceName, result.Error);
        }

        return Ok(result.Value);
    }

    protected async Task<IActionResult> FromCommand(string resourceName, IRequest<UnitResult<Error>> request)
    {
        UnitResult<Error> result = await _mediator.Send(request);

        if (result.IsFailure)
        {
            return Error(resourceName, result.Error);
        }

        return NoContent();
    } 
}