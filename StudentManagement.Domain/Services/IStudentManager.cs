using StudentManagement.Domain.Models.Students;

namespace StudentManagement.Domain.Services
{
    public interface IStudentManager
    {
        public Task<Student> CreateAsync(Name name, Email email, Course course);
        public Task EditPersonalInfoAsync(Student student, Name name, Email email);
    }
}
