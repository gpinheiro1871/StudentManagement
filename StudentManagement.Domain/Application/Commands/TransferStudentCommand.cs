using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class TransferStudentCommand : ICommand
{
    public long StudentId { get; }
    public int EnrollmentNumber { get; }
    public long CourseId { get; }
    public Grade? Grade { get; }

    public TransferStudentCommand(long studentId, 
        int enrollmentNumber, long courseId, Grade? grade)
    {
        StudentId = studentId;
        EnrollmentNumber = enrollmentNumber;
        CourseId = courseId;
        Grade = grade;
    }
}
