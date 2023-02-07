using StudentManagement.Domain.AggregatesModel.Students;

namespace StudentManagement.Api.DataContracts;

public sealed record RegisterRequest
{
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public EnrollmentDto Enrollment { get; init; } = new();

    public record EnrollmentDto
    {
        public long CourseId { get; init; } = 0;
    }
}

public sealed record EditPersonalInfoRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

public sealed record EnrollRequest
{
    public long CourseId { get; init; } = 0;
}

public sealed record GradeRequest
{
    public long CourseId { get; init; } = 0;
    public Grade Grade { get; set; }
}

public sealed record TransferRequest
{
    public long CourseId { get; init; } = 0;
    public Grade? Grade { get; init; } = null;
}

public sealed record DisenrollRequest
{
    public long CourseId { get; init; } = 0;
    public string Comment { get; init; } = string.Empty;
}
