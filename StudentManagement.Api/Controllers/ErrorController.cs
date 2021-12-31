using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ApplicationController
{
    [Route("/error-local-development")]
    public IActionResult ErrorLocalDevelopment(
    [FromServices] IWebHostEnvironment webHostEnvironment)
    {
        if (webHostEnvironment.EnvironmentName != "Development")
        {
            throw new InvalidOperationException(
                "This shouldn't be invoked in non-development environments.");
        }

        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (context is null)
        {
            return Error("Server", Errors.General.InternalServerError());
        }

        return Error("Server", Errors.General.InternalServerError(
            context.Error.Message, context?.Error.StackTrace));
    }

    [Route("/error")]
    public IActionResult Error() => Error("Server", Errors.General.InternalServerError());
}
