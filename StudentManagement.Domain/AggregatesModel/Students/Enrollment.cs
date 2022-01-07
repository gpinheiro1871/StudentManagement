using CSharpFunctionalExtensions;
using StudentManagement.Domain.AggregatesModel.Courses;

namespace StudentManagement.Domain.AggregatesModel.Students;

public class Enrollment : Entity
{
    public virtual Student Student { get; protected set; }
    public virtual Course Course { get; protected set; }
    public virtual Grade? Grade { get; protected set; }

#pragma warning disable CS8618
    protected Enrollment() { }

    public Enrollment(Student student, Course course, Grade? grade)
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

        Student = student;
        Course = course;
        Grade = grade;
    }

    protected internal virtual void SetGrade(Grade grade)
    {
        Grade = grade;
    }

    protected internal virtual void Update(Course course, Grade? grade)
    {
        Course = course;
        Grade = grade;
    }
}

public enum Grade
{
    A = 1,
    B = 2,
    C = 3,
    D = 4,
    F = 5
}
