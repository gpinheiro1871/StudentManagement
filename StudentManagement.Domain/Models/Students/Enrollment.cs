using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Models.Students;

public class Enrollment : Entity
{
    public Student Student { get; protected set; }
    public Course Course { get; protected set; }
    public Grade? Grade { get; protected set; }

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

    public void SetGrade(Grade grade)
    {
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
