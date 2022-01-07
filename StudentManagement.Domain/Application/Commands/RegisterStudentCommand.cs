
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class RegisterStudentCommand : ICommand
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
}