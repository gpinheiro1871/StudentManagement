using StudentManagement.Domain.AggregatesModel.Courses;
using StudentManagement.Domain.AggregatesModel.Students;

namespace StudentManagement.Domain.Services
{
    public class StudentManager : IStudentManager
    {
        private readonly IStudentRepository _studentRepository;

        public StudentManager(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> CreateAsync(Name name, Email email, Course course)
        {
            // Check for email uniqueness
            bool emailExists = await _studentRepository.EmailExistsAsync(email);

            if (emailExists)
            {
                throw new InvalidOperationException("Email exists");
            }

            Student student = Student.Create(name, email, course);

            return student;
        }

        public async Task EditPersonalInfoAsync(Student student, Name name, Email email)
        {
            // Check for email uniqueness
            if (email != student.Email)
            {
                bool emailExists = await _studentRepository.EmailExistsAsync(email);

                if (emailExists)
                {
                    throw new InvalidOperationException("Email exists");
                }
            }

            student.EditPersonalInfo(name, email);
        }
    }
}
