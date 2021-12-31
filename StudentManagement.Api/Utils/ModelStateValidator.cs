using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Utils;

public class ModelStateValidator
{
    public static IActionResult ValidateModelState(ActionContext context)
    {
        Dictionary<string, Error[]> errors = new();

        foreach ((string fieldName, ModelStateEntry entry) in context.ModelState)
        {
            if (entry.Errors.Any())
            {
                Error[] deserializedErrors = entry.Errors.Select(x => Error.Deserialize(x.ErrorMessage))
                    .ToArray();

                errors.Add(fieldName, deserializedErrors);
            }
        }

        return new BadRequestObjectResult(Envelope.Error(errors));
    }
}
