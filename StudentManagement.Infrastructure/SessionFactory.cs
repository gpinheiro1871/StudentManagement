using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.SqlCommand;
using System.Reflection;

namespace StudentManagement.Infrastructure
{
    public sealed class SessionFactory
    {
        private readonly ISessionFactory _factory;

        public SessionFactory(string connectionString)
        {
            _factory = BuildSessionFactory(connectionString);
        }

        internal ISession OpenSession()
        {
            #if DEBUG
                var interceptor = new SqlDebugOutputInterceptor();
                return _factory.WithOptions().Interceptor(interceptor).OpenSession();
            #else
                return _factory.OpenSession();
            #endif
        }

        private static ISessionFactory BuildSessionFactory(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(x => 
                    x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add<IdConvention>()
                    .Conventions.Add<ReferenceConvention>()
                    .Conventions.Add<TableNameConvention>());
            

            return configuration.BuildSessionFactory();
        }

        public class TableNameConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table(instance.EntityType.Name);
            }
        }

        public class IdConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Column("Id");
                instance.GeneratedBy.Native();
            }
        }

        public class ReferenceConvention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.Column(instance.Name + "Id");
            }
        }

        public class SqlDebugOutputInterceptor : EmptyInterceptor
        {
            public override SqlString OnPrepareStatement(SqlString sql)
            {
                Console.Write("NHibernate: ");
                Console.WriteLine(sql);

                return base.OnPrepareStatement(sql);
            }
        }
    }
}
