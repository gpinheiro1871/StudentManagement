using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Infrastructure;
using StudentManagement.Domain.Infrastructure.Repositories;
using StudentManagement.Domain.Utils;
using System.Reflection;
using System.Text;

namespace StudentManagement.Api.Utils;

public static class Dependencies
{
    public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
    {
        var sessionFactory = new SessionFactory(connectionString);

        services.AddSingleton(sessionFactory);
        services.AddScoped(factory => sessionFactory.OpenSession());

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IStudentRepository, StudentRepository>();

        return services;
    }

    //
    // Summary:
    //      Generates a string containing the type name with its generics 
    //      and all its parents in descending order and separated by periods
    // Returns:
    //      TypeA.TypeB<TypeC>
    public static string GetNestedDisplayName(Type x)
    {
        string name = x.Name;

        if (x.IsGenericType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(x.Name.Substring(0, x.Name.IndexOf('`')));
            sb.Append('<');
            bool appendComma = false;
            foreach (Type arg in x.GetGenericArguments())
            {
                if (appendComma) sb.Append(',');
                sb.Append(GetNestedDisplayName(arg));
                appendComma = true;
            }
            sb.Append('>');

            name = sb.ToString();
        }

        if (x.IsNested && x.DeclaringType is not null)
        {
            name = $"{GetNestedDisplayName(x.DeclaringType)}.{name}";
        }

        return name;
    }
}
