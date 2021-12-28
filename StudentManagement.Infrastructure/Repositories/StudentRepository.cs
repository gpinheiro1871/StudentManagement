using NHibernate.Linq;
using StudentManagement.Domain.Models.Students;

namespace StudentManagement.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public StudentRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Student?> GetByIdAsync(long id)
        {
            return _unitOfWork.GetByIdAsync<Student>(id);
        }

        public Task<List<Student>> QueryAll()
        {
            IQueryable<Student> query = _unitOfWork.Query<Student>()
                .FetchLazyProperties();

            return query.ToListAsync();
        }

        public Task SaveAsync(Student student)
        {
            return _unitOfWork.SaveOrUpdateAsync(student);
        }

        public Task DeleteAsync(Student student)
        {
            return _unitOfWork.DeleteAsync(student);
        }

        public Task<Student?> QueryByIdAsync(long id)
        {
            IQueryable<Student> query = _unitOfWork.Query<Student>()
                .FetchLazyProperties();

            return query.Where(x => x.Id == id).FirstOrDefaultAsync() 
                as Task<Student?>;
        }

        public Task<bool> EmailExists(Email email)
        {
            IQueryable<Student> query = _unitOfWork.Query<Student>();

            return query.AnyAsync(x => x.Email == email);
        }
    }
}
