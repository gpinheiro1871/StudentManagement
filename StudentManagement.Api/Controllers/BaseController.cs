using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Utils;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{ 
    protected IActionResult Ok<T>(T result)
    {
        return base.Ok(Envelope.Result(result));
    }

    protected IActionResult Error(string fieldName, Error error)
    {
        // Review
        if (error.Code is Errors.General.InternalServerErrorCode)
        {
            return base.StatusCode(
                StatusCodes.Status500InternalServerError, 
                Envelope.Error(fieldName, error));
        }

        if (error.Code is Errors.General.NotFoundCode)
        {
            return NotFound(Envelope.Error(fieldName, error));
        }

        return base.Ok(Envelope.Error(fieldName, error));
    }
}