using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Application.Dtos;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace StudentManagement.Domain.Application.Queries;

public class GetStudentQuery : IRequest<Result<StudentDto, Error>>
{
    public long StudentId { get; }

    public GetStudentQuery(long id)
    {
        StudentId = id;
    }

    internal sealed class GetStudentQueryHandler 
        : IRequestHandler<GetStudentQuery, Result<StudentDto, Error>>
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Result<StudentDto, Error>> Handle(GetStudentQuery request, 
            CancellationToken cancellationToken)
        {
            Student? student = await _studentRepository.QueryByIdAsync(request.StudentId);

            if (student is null)
            {
                return Errors.General.NotFound(request.StudentId);
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
}
