using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace StudentManagement.Domain.Application.Commands;

public class GradeStudentCommand : IRequest<UnitResult<Error>>
{
    public long StudentId { get; }
    public long CourseId { get; }
    public Grade Grade { get; }

    public GradeStudentCommand(long studentId, 
        long courseId, Grade grade)
    {
        StudentId = studentId;
        CourseId = courseId;
        Grade = grade;
    }

    internal sealed class GradeStudentCommandHandler 
        : IRequestHandler<GradeStudentCommand, UnitResult<Error>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;

        public GradeStudentCommandHandler(IUnitOfWork unitOfWork,
            IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
        }

        public async Task<UnitResult<Error>> Handle(GradeStudentCommand request, 
            CancellationToken cancellationToken)
        {
            Student? student = await _studentRepository.GetByIdAsync(request.StudentId);
            if (student is null)
            {
                return Errors.General.NotFound(request.StudentId);
            }

            Course course = Course.FromId(request.CourseId).Value;

            UnitResult<Error> result = student.Grade(course, request.Grade);
            if (result.IsFailure)
            {
                return result.Error;
            }

            await _unitOfWork.CommitAsync();

            return UnitResult.Success<Error>();
        }
    }
}
