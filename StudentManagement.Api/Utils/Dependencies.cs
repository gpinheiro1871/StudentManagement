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

    public static IServiceCollection RegisterHandlersFromAssembly<T>(this IServiceCollection services)
    {
        var handlers = Assembly.GetAssembly(typeof(T))?
            .GetTypes()
            .Where(t => t.Namespace is not null)
            .Where(t => t.IsClass)
            .Where(t => ImplementHandlerInterfaces(t.GetInterfaces()))
            .ToList();

        if (handlers is null)
        {
            return services;
        }

        var handlerTypes = handlers
            .Select(type => new 
            { 
                handlerInterface = GetHandlerInterface(type),
                handler = type
            })
            .Select(type => ( type.handler, type.handlerInterface));

        foreach (var (handler, handlerInterface) in handlerTypes)
        {
            if (handlerInterface is null)
            {
                throw new ArgumentNullException(nameof(handlerInterface));
            }
            services.AddScoped(handlerInterface, handler);
        }

        return services;
    }

    private static Type? GetHandlerInterface(Type handler)
    {
        var interfaces = handler.GetInterfaces();

        return interfaces.SingleOrDefault(x => IsHandler(x));
    }

    private static bool IsHandler(Type type)
    {
        return type.IsGenericType
            && (type.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
            || type.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
    }

    private static bool ImplementHandlerInterfaces(Type[] types)
    {
        return types.Any(t => IsHandler(t));
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IStudentRepository, StudentRepository>();

        services.AddScoped<Messages>();

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
