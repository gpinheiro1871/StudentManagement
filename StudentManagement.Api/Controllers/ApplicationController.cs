using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Utils;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Controllers;

[ApiController]
public class ApplicationController : ControllerBase
{
    protected IActionResult Ok<T>(T result)
    {
        return base.Ok(Envelope.Result(result));
    }

    protected IActionResult Error(string fieldName, Error error)
    {
        return base.Ok(Envelope.Error(fieldName, error));
    }

    protected IActionResult NotFound(string fieldName, Error error)
    {
        return base.NotFound(Envelope.Error(fieldName, error));
    }

    protected IActionResult CreatedAtAction<T>(
        string actionName, string controllerName, object param, T result)
    {
        return base.CreatedAtAction(
            actionName, controllerName, param, Envelope.Result(result));
    }
}