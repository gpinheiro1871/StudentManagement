using CSharpFunctionalExtensions;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class EnrollStudentCommandHandler : ICommandHandler<EnrollStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentRepository _studentRepository;

    public EnrollStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository)
    {
        _unitOfWork = unitOfWork;
        _studentRepository = studentRepository;
    }

    public async Task<UnitResult<Error>> HandleAsync(EnrollStudentCommand command)
    {
        Student? student = await _studentRepository.GetByIdAsync(command.StudentId);
        if (student is null)
        {
            return Errors.General.NotFound(command.StudentId);
        }

        Course course = Course.FromId(command.CourseId).Value;

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
