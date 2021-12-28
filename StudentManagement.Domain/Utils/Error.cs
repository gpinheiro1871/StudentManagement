using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Utils;

public sealed class Error : ValueObject
{
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

    public static class Errors
    {
        public static class General
        {
            public static Error NotFound(long? id) =>
                id is null 
                ? new Error("record.not.found", "Record not found")
                : new Error("record.not.found", $"Record not found for Id '{id}'");

            public static Error ValueIsInvalid() =>
                new Error("value.is.invalid", "Value is invalid");

            public static Error ValueIsRequired() =>
                new Error("value.is.required", "Value is required");

            public static Error InvalidLength(string? name) =>
                name is null
                ? new Error("invalid.string.length", "Invalid length")
                : new Error("invalid.string.length", $"Invalid {name} length");
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
}
