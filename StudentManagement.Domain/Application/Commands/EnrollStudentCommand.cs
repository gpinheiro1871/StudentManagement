using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class EnrollStudentCommand : ICommand
{
    public long StudentId { get; }
    public long CourseId { get; }

    public EnrollStudentCommand(long studentId, long courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
    }
}
