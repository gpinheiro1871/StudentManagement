using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace StudentManagement.Domain.Application.Commands;

public class TransferStudentCommand : IRequest<UnitResult<Error>>
{
    public long StudentId { get; }
    public int EnrollmentNumber { get; }
    public long CourseId { get; }
    public Grade? Grade { get; }

    public TransferStudentCommand(long studentId, 
        int enrollmentNumber, long courseId, Grade? grade)
    {
        StudentId = studentId;
        EnrollmentNumber = enrollmentNumber;
        CourseId = courseId;
        Grade = grade;
    }

    internal sealed class TransferStudentCommandHandler 
        : IRequestHandler<TransferStudentCommand, UnitResult<Error>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;

        public TransferStudentCommandHandler(IUnitOfWork unitOfWork, 
            IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
        }

        public async Task<UnitResult<Error>> Handle(TransferStudentCommand request, 
            CancellationToken cancellationToken)
        {
            Student? student = await _studentRepository.GetByIdAsync(request.StudentId);
            if (student is null)
            {
                return Errors.General.NotFound(request.StudentId);
            }

            Course course = Course.FromId(request.CourseId).Value;

            var result = student.Transfer(request.EnrollmentNumber, course, request.Grade);
            if (result.IsFailure)
            {
                return result.Error;
            }

            await _unitOfWork.CommitAsync();

            return UnitResult.Success<Error>();
        }
    }
}
