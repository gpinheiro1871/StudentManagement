using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class GradeStudentCommand : ICommand
{
    public long StudentId { get; }
    public long CourseId { get; }
    public Grade Grade { get; }

    public GradeStudentCommand(long studentId, 
        long courseId, Grade grade)
    {
        StudentId = studentId;
        CourseId = courseId;
        Grade = grade;
    }
}
