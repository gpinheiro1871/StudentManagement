namespace StudentManagement.Domain.Models.Students;

public interface IStudentRepository
{
    Task<Student?> QueryByIdAsync(long id);
    Task<List<Student>> QueryAll();
    Task<Student?> GetByIdAsync(long id);
    Task SaveAsync(Student student);
    Task DeleteAsync(Student student);
    Task<bool> EmailExists(Email email);
}
