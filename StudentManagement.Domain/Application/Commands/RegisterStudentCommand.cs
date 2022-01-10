using Domain = StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace StudentManagement.Domain.Application.Commands;

public class RegisterStudentCommand : IRequest<UnitResult<Error>>
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public long FirstEnrollmentCourseId { get; }

    public RegisterStudentCommand(string firstName, string lastName, 
        string email, long firstEnrollmentCourseId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        FirstEnrollmentCourseId = firstEnrollmentCourseId;
    }

    internal sealed class RegisterStudentCommandHandler
        : IRequestHandler<RegisterStudentCommand, UnitResult<Error>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterStudentCommandHandler(IUnitOfWork unitOfWork,
            IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<Error>> Handle(RegisterStudentCommand request, 
            CancellationToken cancellationToken)
        {
            Course course = Course.FromId(request.FirstEnrollmentCourseId).Value;

            Name name = Name.Create(request.FirstName, request.LastName).Value;

            Email email = Domain::Email.Create(request.Email).Value;

            // Check for email uniqueness
            bool emailExists = await _studentRepository.EmailExistsAsync(email);
            if (emailExists)
            {
                return Errors.Student.EmailIsTaken();
            }

            Student student = new(name, email, course);

            await _studentRepository.SaveAsync(student);

            await _unitOfWork.CommitAsync();

            return UnitResult.Success<Error>();
        }
    }
}