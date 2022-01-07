using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands
{
    public class DisenrollStudentCommand : ICommand
    {
        public long StudentId { get; }
        public long CourseId { get; }
        public string Comment { get; }

        public DisenrollStudentCommand(
            long studentId, long courseId, string comment)
        {
            StudentId = studentId;
            CourseId = courseId;
            Comment = comment;
        }
    }
}
