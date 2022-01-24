namespace StudentManagement.Domain.Application.Queries.Responses;

public record GetStudentListResponse
{
    public List<Student>? Students { get; internal set; }

    public record Student
    {
        public long Id { get; internal set; }
        public string? Name { get; internal set; }
        public List<Enrollment>? Enrollments { get; internal set; }
    }

    public record Enrollment
    {
        public long CourseId { get; internal set; }
        public string? CourseName { get; internal set; }
    }
}