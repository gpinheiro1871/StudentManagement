using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Models.Students
{
    public class Disenrollment : Entity
    {
        public virtual Student Student { get; protected set; }
        public virtual Course Course { get; protected set; }
        public virtual DateTime DateTime { get; protected set; }
        public virtual string Comment { get; protected set; }

        #pragma warning disable CS8618
        protected Disenrollment() { }

        public Disenrollment(Student student, Course course, string comment)
            : this()
        {
            if (student is null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            if (course is null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentNullException(nameof(course));
            }

            Student = student;
            Course = course;
            Comment = comment;
            DateTime = DateTime.UtcNow;
        }
    }
}