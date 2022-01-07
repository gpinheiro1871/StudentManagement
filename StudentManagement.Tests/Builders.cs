using Moq;
using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Services;
using System.Threading.Tasks;

namespace StudentManagement.Tests;

public class Builders
{
    public static async Task<Student> buildStudent()
    {

        var name = Name.Create("Lucas", "Smith").Value;
        var email = Email.Create("lucass@gmail.com").Value;
        var course = Course.FromId(1L).Value;

        Mock<IStudentRepository> _repositoryStub = new();
        _repositoryStub.Setup(x => x.EmailExistsAsync(email)).ReturnsAsync(false);

        StudentManager studentManager = new(_repositoryStub.Object);

        var student = await studentManager.CreateAsync(name, email, course);

        return student;
    }
}
