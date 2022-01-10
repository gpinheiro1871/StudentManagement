using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace StudentManagement.Domain.Application.Commands;

public class EnrollStudentCommand : IRequest<UnitResult<Error>>
{
    public long StudentId { get; }
    public long CourseId { get; }

    public EnrollStudentCommand(long studentId, 
        long courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
    }

    internal sealed class EnrollStudentCommandHandler 
        : IRequestHandler<EnrollStudentCommand, UnitResult<Error>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;

        public EnrollStudentCommandHandler(IUnitOfWork unitOfWork, 
            IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
        }

        public async Task<UnitResult<Error>> Handle(EnrollStudentCommand request, 
            CancellationToken cancellationToken)
        {
            Student? student = await _studentRepository.GetByIdAsync(request.StudentId);
            if (student is null)
            {
                return Errors.General.NotFound(request.StudentId);
            }

            Course course = Course.FromId(request.CourseId).Value;

            var canEnroll = student.CanEnroll(course);

            if (canEnroll.IsFailure)
            {
                return canEnroll.Error;
            }

            student.Enroll(course);

            await _unitOfWork.CommitAsync();

            return UnitResult.Success<Error>();
        }
    }
}
