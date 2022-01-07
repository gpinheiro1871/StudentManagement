using NHibernate;
using NHibernate.Linq;
using StudentManagement.Domain.AggregatesModel.Students;

namespace StudentManagement.Domain.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ISession _session;

        public StudentRepository(ISession session)
        {
            _session = session;
        }

        public Task<Student?> GetByIdAsync(long id)
        {
            return _session.GetAsync<Student?>(id);
        }

        public Task<List<Student>> QueryAllAsync()
        {
            IQueryable<Student> query = _session.Query<Student>();

            return query.Fetch(x => x.Enrollments).ToListAsync();
        }

        public Task SaveAsync(Student student)
        {
            return _session.SaveOrUpdateAsync(student);
        }

        public Task DeleteAsync(Student student)
        {
            return _session.DeleteAsync(student);
        }

        public Task<Student?> QueryByIdAsync(long id)
        {
            IQueryable<Student> query = _session.Query<Student>();

            return query.Where(x => x.Id == id)
                .FetchMany(x => x.Enrollments)
                .SingleOrDefaultAsync()
                as Task<Student?>;
        }

        public Task<bool> EmailExistsAsync(Email email)
        {
            IQueryable<Student> query = _session.Query<Student>();

            return query.AnyAsync(x => x.Email == email);
        }
    }
}
