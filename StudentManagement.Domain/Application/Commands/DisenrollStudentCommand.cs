using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace StudentManagement.Domain.Application.Commands;

public class DisenrollStudentCommand : IRequest<UnitResult<Error>>
{
    public long StudentId { get; }
    public long CourseId { get; }
    public string Comment { get; }

    public DisenrollStudentCommand(long studentId, 
        long courseId, string comment)
    {
        StudentId = studentId;
        CourseId = courseId;
        Comment = comment;
    }

    internal sealed class DisenrollStudentCommandHandler
        : IRequestHandler<DisenrollStudentCommand, UnitResult<Error>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;

        public DisenrollStudentCommandHandler(IUnitOfWork unitOfWork, 
            IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
        }

        public async Task<UnitResult<Error>> Handle(DisenrollStudentCommand request,
            CancellationToken cancellationToken)
        {
            //find student
            Student? student = await _studentRepository.GetByIdAsync(request.StudentId);
            if (student is null)
            {
                return Errors.General.NotFound(request.StudentId);
            }

            Course course = Course.FromId(request.CourseId).Value;

            var result = student.Disenroll(course, request.Comment);
            if (result.IsFailure)
            {
                return result.Error;
            }

            await _unitOfWork.CommitAsync();

            return UnitResult.Success<Error>();
        }
    }
}
