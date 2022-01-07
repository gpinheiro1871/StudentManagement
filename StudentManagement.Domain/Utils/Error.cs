using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Utils;

public sealed class Error : ValueObject
{
    private const string Separator = "||";

    // Message here is for debug purposes only
    // Clients should not show Message to users,
    // map your own error message from this error Code instead!
    public string Code { get; }
    public string Message { get; }

    internal Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }

    public string Serialize()
    {
        return $"{Code}{Separator}{Message}";
    }

    public static Error? Deserialize(string serialized)
    {
        if (serialized == "A non-empty request body is required.")
            return Errors.General.ValueIsRequired(null);

        string[] data = serialized.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

        if (data.Length < 2)
            return null;

        return new Error(data[0], data[1]);
    }
}

public static class Errors
{
    public static class General
    {
        public const string NotFoundCode = "record.not.found";

        public static Error ClientError() =>
            new Error("client.error", "Request is invalid");

        public static Error NotFound(long? id) =>
            id is null
            ? new Error(NotFoundCode, "Record not found")
            : new Error(NotFoundCode, $"Record not found for Id '{id}'");

        public static Error ValueIsInvalid(string? name) =>
            name is null
            ? new Error("value.is.invalid", "Value is invalid")
            : new Error("value.is.invalid", $"Value is invalid for {name}");

        public static Error ValueIsRequired(string? name) =>
            name is null
            ? new Error("value.is.required", "Value is required")
            : new Error("value.is.required", $"Value is required for {name}");

        public static Error InvalidLength(string? name) =>
            name is null
            ? new Error("invalid.string.length", "Invalid length")
            : new Error("invalid.string.length", $"Invalid {name} length");

        public static Error InternalServerError() =>
            new Error("internal.server.error", "Internal server error");

        public static Error InternalServerError(string message, string? stackTrace) =>
            stackTrace is null
            ? new Error("internal.server.error", $"{message}")
            : new Error("internal.server.error", $"{message}: \\n {stackTrace}");
    }

    public static class Student
    {
        public static Error TooManyEnrollments() =>
            new Error("student.too.many.enrollments", "Student cannot have more than 2 enrollments");

        public static Error AlreadyEnrolled(string courseName) =>
            new Error("student.already.enrolled", $"Student is already enrolled into course '{courseName}'");

        public static Error EmailIsTaken() =>
            new Error("student.email.is.taken", $"Student email is taken");

        public static UnitResult<Error> NotEnrolled(string courseName) =>
            new Error("student.not.enrolled", $"Student is not enrolled into course '{courseName}'");

        public static UnitResult<Error> InvalidEnrollmentNumber() =>
            new Error("student.invalid.enrollment.number", "Student enrollment number is invalid");

        public static UnitResult<Error> EnrollmentNumberNotFound(int enrollmentNumber) =>
            new Error("student.enrollment.number.not.found", $"Student enrollment not found for enrollment number '{enrollmentNumber}'");
    }
}
