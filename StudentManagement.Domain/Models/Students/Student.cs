using CSharpFunctionalExtensions;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Models.Students;

public class Student : 
    Entity, 
    IAggregateRoot
{
    public virtual Name Name { get; protected set; }


    private string _email;
    public virtual Email Email
    {
        get => Email.Create(_email).Value;
        protected set => _email = value.Value;
    }

    private readonly IList<Enrollment> _enrollments = new List<Enrollment>();
    public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

    public virtual Enrollment FirstEnrollment => GetEnrollment(0);
    public virtual Enrollment? SecondEnrollment => GetEnrollment(1);

    private readonly IList<Disenrollment> _disenrollments = new List<Disenrollment>();
    public virtual IReadOnlyList<Disenrollment> Disenrollments => _disenrollments.ToList();

    #pragma warning disable CS8618
    protected Student() { }

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

    public virtual void Enroll(Course course)
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

    public virtual void Grade(Course course, Grade grade)
    {
        var enrollment = _enrollments.Single(x => x.Course == course);

        enrollment.SetGrade(grade);
    }

    public virtual void Disenroll(Course course, string comment)
    {
        Enrollment? enrollment = _enrollments.FirstOrDefault(y => y.Course == course);
        if (enrollment is null)
        {
            return;
        }
        _enrollments.Remove(enrollment);

        Disenrollment disenrollment = new(this, course, comment);

        _disenrollments.Add(disenrollment);
    }

    private Enrollment GetEnrollment(int index)
    {
        if (_enrollments.Count > index)
            return _enrollments[index];

        return null;
    }

    public virtual void EditPersonalInfo(Name name, Email email)
    {
        Name = name;
        Email = email;
    }

    public virtual void Transfer(int enrollmentNumber, Course course, Grade? grade)
    {
        if (enrollmentNumber > 2 || enrollmentNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(enrollmentNumber));
        }

        Enrollment enrollment = _enrollments.ElementAt(enrollmentNumber-1);

        if (enrollment is null)
        {
            return;
        }

        enrollment.Update(course, grade);

        return;
    }
}
               