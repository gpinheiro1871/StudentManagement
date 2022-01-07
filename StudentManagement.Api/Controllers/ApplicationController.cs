using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Controllers;

[ApiController]
public abstract class ApplicationController : BaseController
{
    private readonly Messages _messages;

    protected ApplicationController(Messages messages)
    {
        _messages = messages;
    }

    protected async Task<IActionResult> FromQuery<T>(string resourceName, IQuery<T> query)
    {
        Result<T, Error> result = await _messages.DispatchAsync(query);

        if (result.IsFailure)
        {
            return Error(resourceName, result.Error);
        }

        return Ok(result.Value);
    }

    protected async Task<IActionResult> FromCommand(string resourceName, ICommand command)
    {
        UnitResult<Error> result = await _messages.DispatchAsync(command);

        if (result.IsFailure)
        {
            return Error(resourceName, result.Error);
        }

        return NoContent();
    }
}