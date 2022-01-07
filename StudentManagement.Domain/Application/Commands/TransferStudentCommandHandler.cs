using CSharpFunctionalExtensions;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class TransferStudentCommandHandler : ICommandHandler<TransferStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentRepository _studentRepository;

    public TransferStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository)
    {
        _unitOfWork = unitOfWork;
        _studentRepository = studentRepository;
    }

    public async Task<UnitResult<Error>> HandleAsync(TransferStudentCommand command)
    {
        Student? student = await _studentRepository.GetByIdAsync(command.StudentId);
        if (student is null)
        {
            return Errors.General.NotFound(command.StudentId);
        }

        Course course = Course.FromId(command.CourseId).Value;

        var result = student.Transfer(command.EnrollmentNumber, course, command.Grade);
        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.CommitAsync();

        return UnitResult.Success<Error>();
    }
}
