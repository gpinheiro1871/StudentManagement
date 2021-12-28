using CSharpFunctionalExtensions;
using StudentManagement.Domain.Models.Students;
using StudentManagement.Domain.Utils;
using static StudentManagement.Domain.Utils.Error;

namespace StudentManagement.Domain.Services
{
    public class StudentManager : IStudentManager
    {
        private readonly IStudentRepository _studentRepository;

        public StudentManager(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Result<Student, Error>> Create(Name name, Email email, Course course)
        {
            // Check for email uniqueness
            bool emailExists = await _studentRepository.EmailExists(email);

            if (emailExists)
            {
                return Errors.Student.EmailIsTaken();
            }

            Student student = Student.Create(name, email, course);

            return student;
        }

        public async Task<UnitResult<Error>> EditPersonalInfo(Student student, Name name, Email email)
        {
            // Check for email uniqueness
            bool emailExists = await _studentRepository.EmailExists(email);

            if (emailExists)
            {
                return Errors.Student.EmailIsTaken();
            }

            student.EditPersonalInfo(name, email);

            return UnitResult.Success<Error>();
        }
    }
}
