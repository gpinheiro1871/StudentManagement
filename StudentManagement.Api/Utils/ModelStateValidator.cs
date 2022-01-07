using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StudentManagement.Domain.Utils;
using System.Linq;

namespace StudentManagement.Api.Utils;

public class ModelStateValidator
{
    public static IActionResult ValidateModelState(ActionContext context)
    {
        Dictionary<string, Error[]> errors = new();
        bool clientUnknownError = false;

        foreach ((string fieldName, ModelStateEntry entry) in context.ModelState)
        {
            if (!entry.Errors.Any())
            {
                continue;
            }

            List<Error> deserializedErrors = DeserializeErrors(ref clientUnknownError, entry);

            if (clientUnknownError)
            {
                return new BadRequestObjectResult(Envelope.Error("Request", Errors.General.ClientError()));
            }

            errors.Add(fieldName, deserializedErrors.ToArray());
        }

        return new BadRequestObjectResult(Envelope.Error(errors));
    }

    private static List<Error> DeserializeErrors(ref bool clientUnknownError, ModelStateEntry entry)
    {
        List<Error> deserializedErrors = new();
        foreach (var error in entry.Errors)
        {
            var deserializedError = Error.Deserialize(error.ErrorMessage);

            if (deserializedError is null)
            {
                clientUnknownError = true;
                return deserializedErrors;
            }

            deserializedErrors.Add(deserializedError);
        }

        return deserializedErrors;
    }
}
