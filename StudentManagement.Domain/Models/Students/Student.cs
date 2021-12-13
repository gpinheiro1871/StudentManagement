using CSharpFunctionalExtensions;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Models.Students;

public class Student : 
    Entity<Guid>, 
    IAggregateRoot
{
    public Name Name { get; set; }
    public Email Email { get; set; }

    private readonly List<Enrollment> _enrollments = new List<Enrollment>();
    public IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();


    private readonly List<Disenrollment> _disenrollments = new List<Disenrollment>();
    public IReadOnlyList<Disenrollment> Disenrollments => _disenrollments.ToList();


    public Student(Name name, Email email, Course course)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (email is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        Name = name;
        Email = email;
        Enroll(course);
    }

    public void Enroll(Course course)
    {
        if (_enrollments.Count == 2)
        {
            throw new InvalidOperationException();
        }

        if (_enrollments.Any(x => x.Course == course))
        {
            throw new InvalidOperationException();
        }

        _enrollments.Add(new Enrollment(this, course, null));
    }

    public void Grade(Course course, Grade grade)
    {
        var enrollment = _enrollments.Single(x => x.Course == course);

        enrollment.SetGrade(grade);
    }

    public void Disenroll(Course course, string comment)
    {
        Enrollment enrollment = _enrollments.Single(y => y.Course == course);

        Disenrollment disenrollment = new(this, course, comment);

        _enrollments.Remove(enrollment);

        _disenrollments.Add(disenrollment);
    }
}
               