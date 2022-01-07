using CSharpFunctionalExtensions;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class RegisterStudentCommandHandler
    : ICommandHandler<RegisterStudentCommand>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterStudentCommandHandler(IUnitOfWork unitOfWork, 
        IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<Error>> HandleAsync(RegisterStudentCommand command)
    {
        Course course = Course.FromId(command.FirstEnrollmentCourseId).Value;

        Name name = Name.Create(command.FirstName, command.LastName).Value;

        Email email = Email.Create(command.Email).Value;

        // Check for email uniqueness
        bool emailExists = await _studentRepository.EmailExistsAsync(email);

        if (emailExists)
        {
            Errors.Student.EmailIsTaken();
        }

        Student student = new Student(name, email, course);

        await _studentRepository.SaveAsync(student);

        await _unitOfWork.CommitAsync();

        return UnitResult.Success<Error>();
    }
}
