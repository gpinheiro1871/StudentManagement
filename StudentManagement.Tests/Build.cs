using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.AggregatesModel.Students;

namespace StudentManagement.Tests;

public static class Build
{
    public static Student Student()
    {
        Name name = Name.Create("Lucas", "Smith").Value;
        Email email = Email.Create("lucass@gmail.com").Value;
        Course course = Course.FromId(1L).Value;

        var student = new Student(name, email, course);

        return student;
    }
}
