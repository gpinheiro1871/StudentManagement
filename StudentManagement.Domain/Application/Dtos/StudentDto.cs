using Newtonsoft.Json;

namespace StudentManagement.Domain.Application.Dtos;

#pragma warning disable CS8618 
[JsonObject(ItemNullValueHandling = NullValueHandling.Include)]
public sealed record StudentDto
{
    public long Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }

    public EnrollmentDto? FirstEnrollment { get; init; }
    public EnrollmentDto? SecondEnrollment { get; init; }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Include)]
    public sealed record EnrollmentDto
    {
        public long CourseId { get; init; }
        public string CourseName { get; init; }
        public string? Grade { get; init; }
    }
}
