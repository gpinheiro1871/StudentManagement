namespace StudentManagement.Domain.Utils;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
}
