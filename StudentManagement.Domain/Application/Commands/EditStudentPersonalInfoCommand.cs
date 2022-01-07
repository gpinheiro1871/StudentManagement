using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public sealed class EditStudentPersonalInfoCommand : ICommand
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
}