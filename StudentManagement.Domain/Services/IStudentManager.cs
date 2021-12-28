using CSharpFunctionalExtensions;
using StudentManagement.Domain.Models.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Services
{
    public interface IStudentManager
    {
        public Task<Result<Student, Error>> Create(Name name, Email email, Course course);
        Task<UnitResult<Error>> EditPersonalInfo(Student student, Name name, Email email);
    }
}
