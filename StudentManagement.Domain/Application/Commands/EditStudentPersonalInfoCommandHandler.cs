using CSharpFunctionalExtensions;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public sealed class EditStudentPersonalInfoCommandHandler
    : ICommandHandler<EditStudentPersonalInfoCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentRepository _studentRepository;

    public EditStudentPersonalInfoCommandHandler(IUnitOfWork unitOfWork, 
        IStudentRepository studentRepository)
    {
        _unitOfWork = unitOfWork;
        _studentRepository = studentRepository;
    }

    public async Task<UnitResult<Error>> HandleAsync(EditStudentPersonalInfoCommand command)
    {
        //Find student
        Student? student = await _studentRepository.GetByIdAsync(command.Id);
        if (student is null)
        {
            return Errors.General.NotFound(command.Id);
        }

        Name name = Name.Create(command.FirstName, command.LastName).Value;

        Email email = Email.Create(command.Email).Value;

        // Check for email uniqueness
        if (email != student.Email)
        {
            bool emailExists = await _studentRepository.EmailExistsAsync(email);

            if (emailExists)
            {
                return Errors.Student.EmailIsTaken();
            }
        }

        student.EditPersonalInfo(name, email);

        await _unitOfWork.CommitAsync();

        return UnitResult.Success<Error>();
    }
}