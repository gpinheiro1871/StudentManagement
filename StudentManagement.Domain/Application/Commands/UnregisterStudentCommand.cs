using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class UnregisterStudentCommand : ICommand
{
    public long Id { get; }

    public UnregisterStudentCommand(long id)
    {
        Id = id;
    }
}
