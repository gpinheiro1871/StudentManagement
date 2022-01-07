using CSharpFunctionalExtensions;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.AggregatesModel.Students;
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

    public virtual Enrollment? FirstEnrollment => 
        _enrollments.ElementAtOrDefault(0);
    public virtual Enrollment? SecondEnrollment => 
        _enrollments.ElementAtOrDefault(1);

    private readonly IList<Disenrollment> _disenrollments = new List<Disenrollment>();
    public virtual IReadOnlyList<Disenrollment> Disenrollments => _disenrollments.ToList();

#pragma warning disable CS8618
    protected Student() { }
    private Student(Name name, Email email, Course course)
    {
        Name = name;
        Email = email;
    }
#pragma warning restore CS8618

    internal static Student Create(Name name, Email email, Course course)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (email is null)
        {
            throw new ArgumentNullException(nameof(email));
        }
        if (course is null)
        {
            throw new ArgumentNullException(nameof(email));
        }

        Student student = new Student(name, email, course);

        student.Enroll(course);

        return student;
    }

    public virtual UnitResult<Error> CanEnroll(Course course)
    {
        if (_enrollments.Any(x => x.Course == course))
        {
            return Errors.Student.AlreadyEnrolled(course.Name);
        }

        if (_enrollments.Count == 2)
        {
            return Errors.Student.TooManyEnrollments();
        }

        return UnitResult.Success<Error>();
    }

    public virtual void Enroll(Course course)
    {
        if (CanEnroll(course).IsFailure)
        {
            throw new InvalidOperationException("CanEnroll not called");
        }

        _enrollments.Add(new Enrollment(this, course, null));
    }

    public virtual UnitResult<Error> Grade(Course course, Grade grade)
    {
        Enrollment? enrollment = _enrollments.FirstOrDefault(x => x.Course == course);

        if (enrollment is null)
        {
            return Errors.Student.NotEnrolled(course.Name);
        }

        enrollment.SetGrade(grade);

        return UnitResult.Success<Error>();
    }

    public virtual UnitResult<Error> Disenroll(Course course, string comment)
    {
        Enrollment? enrollment = _enrollments.FirstOrDefault(y => y.Course == course);

        if (enrollment is null)
        {
            return Errors.Student.NotEnrolled(course.Name);
        }
        _enrollments.Remove(enrollment);

        Disenrollment disenrollment = new(this, course, comment);

        _disenrollments.Add(disenrollment);

        return UnitResult.Success<Error>();
    }

    protected internal virtual void EditPersonalInfo(Name name, Email email)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (email is null)
        {
            throw new ArgumentNullException(nameof(email));
        }

        Name = name;
        Email = email;
    }

    public virtual UnitResult<Error> Transfer(int enrollmentNumber, Course course, Grade? grade)
    {
        if (enrollmentNumber > 2 || enrollmentNumber < 1)
        {
            return Errors.Student.InvalidEnrollmentNumber();
        }

        Enrollment? enrollment = _enrollments.ElementAtOrDefault(enrollmentNumber - 1);

        if (enrollment is null)
        {
            return Errors.Student.EnrollmentNumberNotFound(enrollmentNumber);
        }

        enrollment.Update(course, grade);

        return UnitResult.Success<Error>();
    }
}
