using Domain = StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace StudentManagement.Domain.Application.Commands;

public sealed class EditStudentPersonalInfoCommand : IRequest<UnitResult<Error>>
{
    public long Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }

    public EditStudentPersonalInfoCommand(long id, string firstName,
        string lastName, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    internal sealed class EditStudentPersonalInfoCommandHandler
        : IRequestHandler<EditStudentPersonalInfoCommand, UnitResult<Error>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;

        public EditStudentPersonalInfoCommandHandler(IUnitOfWork unitOfWork,
            IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
        }

        public async Task<UnitResult<Error>> Handle(EditStudentPersonalInfoCommand request, 
            CancellationToken cancellationToken)
        {
            //Find student
            Student? student = await _studentRepository.GetByIdAsync(request.Id);
            if (student is null)
            {
                return Errors.General.NotFound(request.Id);
            }

            Name name = Name.Create(request.FirstName, request.LastName).Value;

            Email email = Domain::Email.Create(request.Email).Value;

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
}