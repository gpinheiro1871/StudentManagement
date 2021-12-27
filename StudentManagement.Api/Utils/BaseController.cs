using Microsoft.AspNetCore.Mvc;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Utils;

public class BaseController : ControllerBase
{
    protected IActionResult Ok<T>(T value)
    {
        return base.Ok(ApiResponse.From(value));
    }


    protected IActionResult FromDomainResult<T>(DomainResult<T> domainResult)
    {
        return Reply(ApiResponse.From(domainResult));
    }
    
    protected IActionResult FromDomainActionResult(DomainActionResult domainActionResult)
    {
        return Reply(ApiResponse.From(domainActionResult));
    }

    private IActionResult Reply<T>(ApiResponse<T> response)
    {
        if (response.Success)
        {
            return base.Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }
}
