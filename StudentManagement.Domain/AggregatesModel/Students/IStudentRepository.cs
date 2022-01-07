namespace StudentManagement.Domain.AggregatesModel.Students;

public interface IStudentRepository
{
    Task<Student?> QueryByIdAsync(long id);
    Task<List<Student>> QueryAllAsync();
    Task<Student?> GetByIdAsync(long id);
    Task SaveAsync(Student student);
    Task DeleteAsync(Student student);
    Task<bool> EmailExistsAsync(Email email);
}