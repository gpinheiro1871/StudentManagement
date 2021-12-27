using StudentManagement.Domain.Models.Students;

namespace StudentManagement.Api.Dtos;

public sealed record StudentDto
{
    public long Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }

    public EnrollmentDto FirstEnrollment { get; init; }
    public EnrollmentDto? SecondEnrollment { get; init; }

    public sealed record EnrollmentDto
    {
        public long CourseId { get; init; }
        public string CourseName { get; init; }
        public string? Grade { get; init; }
    }
}

public sealed record RegisterRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public EnrollmentDto Enrollment { get; init; }

    public class EnrollmentDto
    {
        public long CourseId { get; init; }
    }
}

public sealed record EditPersonalInfoRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
}

public sealed record EnrollRequest
{
    public long CourseId { get; init; }
}

public sealed record GradeRequest
{
    public long CourseId { get; init; }
    public Grade Grade { get; init; }
}

public sealed record TransferRequest
{
    public long CourseId { get; init; }
    public Grade Grade { get; init; }
}

public sealed record DisenrollRequest
{
    public long CourseId { get; init; }
    public string Comment { get; init; }
}