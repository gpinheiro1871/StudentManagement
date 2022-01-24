using StudentManagement.Domain.AggregatesModel.Students;

namespace StudentManagement.Domain.Application.Queries.Responses;

public sealed record GetStudentQueryResponse
{
    public long Id { get; internal set; }
    public string? FirstName { get; internal set; }
    public string? LastName { get; internal set; }
    public string? Email { get; internal set; }

    public Enrollment? FirstEnrollment { get; internal set; }
    public Enrollment? SecondEnrollment { get; internal set; }

    public List<Disenrollment>? Disenrollments { get; internal set; }

    public sealed record Enrollment
    {
        public long CourseId { get; internal set; }
        public string? CourseName { get; internal set; }
        public Grade? Grade { get; internal set; }
    }

    public sealed record Disenrollment
    {
        public long CourseId { get; internal set; }
        public string? CourseName { get; internal set; }
        public string? Comment { get; internal set; }
    }
}