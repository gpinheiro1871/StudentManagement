using NHibernate;
using StudentManagement.Domain.Utils;
using System.Data;

namespace StudentManagement.Infrastructure.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;
        private bool _isAlive = true;

        public UnitOfWork(SessionFactory sessionFactory)
        {
            _session = sessionFactory.OpenSession();
            _transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public Task CommitAsync()
        {
            if (!_isAlive)
                return Task.CompletedTask;

            try
            {
                return _transaction.CommitAsync();
            }
            finally
            {
                _isAlive = false;
                _transaction.Dispose();
                _session.Dispose();
            }
        }

        public Task<T?> GetByIdAsync<T>(long id)
            where T : class
        {
            return _session.GetAsync<T?>(id);
        }

        public Task SaveOrUpdateAsync<T>(T entity)
        {
            return _session.SaveOrUpdateAsync(entity);
        }

        public Task DeleteAsync<T>(T entity)
        {
            return _session.DeleteAsync(entity);
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }

        public ISQLQuery CreateSQLQuery(string q)
        {
            return _session.CreateSQLQuery(q);
        }
    }
}
