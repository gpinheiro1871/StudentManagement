using NHibernate;
using StudentManagement.Domain.Utils;
using System.Data;

namespace StudentManagement.Domain.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;
        private bool _isAlive = true;

        public UnitOfWork(ISession session)
        {
            _session = session;
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
                Dispose();
            }
        }

        public void Dispose()
        {
            _isAlive = false;
            _transaction.Dispose();
            _session.Dispose();
        }
    }
}
