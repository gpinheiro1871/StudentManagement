using StudentManagement.Domain.Models.Students;

namespace StudentManagement.Tests;

public class Builders
{
    public static Student buildStudent()
    {
        var nameResult = Name.Create("Lucas", "Smith");
        var emailResult = Email.Create("lucass@gmail.com");
        var course = Course.FromId(1L);
        var student = new Student(nameResult.Value, emailResult.Value, course);

        return student;
    }
}
