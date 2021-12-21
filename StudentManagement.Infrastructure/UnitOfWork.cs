using NHibernate;
using System.Data;

namespace StudentManagement.Infrastructure
{
    public class UnitOfWork
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

        internal Task<T?> GetByIdAsync<T>(long id)
            where T : class
        {
            return _session.GetAsync<T?>(id);
        }

        internal Task SaveOrUpdateAsync<T>(T entity)
        {
            return _session.SaveOrUpdateAsync(entity);
        }

        internal Task DeleteAsync<T>(T entity)
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
