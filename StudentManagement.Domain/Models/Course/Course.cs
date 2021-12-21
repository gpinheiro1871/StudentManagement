using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Models.Students;

public class Course : Entity
{
    public static readonly Course Calculus = new Course(1, "Calculus");
    public static readonly Course Chemistry = new Course(2, "Chemistry");
    public static readonly Course English = new Course(3, "English");

    public static readonly Course[] AllCourses = { Calculus, Chemistry, English };

    public virtual string Name { get; }


    #pragma warning disable CS8618
    protected Course() { }

    private Course(long id, string name)
        : base(id)
    {
        Name = name;
    }

    public static Course? FromId(long id)
    {
        return AllCourses.SingleOrDefault(x => x.Id == id);
    }
}
