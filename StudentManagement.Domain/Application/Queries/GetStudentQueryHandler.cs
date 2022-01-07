using CSharpFunctionalExtensions;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Application.Dtos;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Queries;

public class GetStudentQueryHandler : IQueryHandler<GetStudentQuery, StudentDto>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result<StudentDto, Error>> HandleAsync(GetStudentQuery query)
    {
        Student? student = await _studentRepository.QueryByIdAsync(query.Id);

        if (student is null)
        {
            return Errors.General.NotFound(query.Id);
        }

        return ConvertToDto(student);
    }

    private StudentDto ConvertToDto(Student student)
    {
        var firstEnrollment = student.FirstEnrollment is null
            ? null
            : new StudentDto.EnrollmentDto()
            {
                CourseId = student.FirstEnrollment.Course.Id,
                CourseName = student.FirstEnrollment.Course.Name,
                Grade = student.FirstEnrollment.Grade?.ToString()
            };

        var secondEnrollment = student.SecondEnrollment is null
            ? null
            : new StudentDto.EnrollmentDto()
            {
                CourseId = student.SecondEnrollment.Course.Id,
                CourseName = student.SecondEnrollment.Course.Name,
                Grade = student.SecondEnrollment.Grade?.ToString()
            };

        return new StudentDto
        {
            Id = student.Id,
            Name = student.Name.ToString(),
            Email = student.Email.ToString(),
            FirstEnrollment = firstEnrollment,
            SecondEnrollment = secondEnrollment
        };
    }
}
