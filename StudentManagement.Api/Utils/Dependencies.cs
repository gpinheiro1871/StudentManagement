using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Application.Commands;
using StudentManagement.Domain.Application.Dtos;
using StudentManagement.Domain.Application.Queries;
using StudentManagement.Domain.Infrastructure;
using StudentManagement.Domain.Infrastructure.Repositories;
using StudentManagement.Domain.Services;
using StudentManagement.Domain.Utils;

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

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetStudentListQuery, List<StudentDto>>, GetStudentListQueryHandler>();
        services.AddScoped<IQueryHandler<GetStudentQuery, StudentDto>, GetStudentQueryHandler>();

        return services;
    } 
    
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<RegisterStudentCommand>, RegisterStudentCommandHandler>();
        services.AddScoped<ICommandHandler<UnregisterStudentCommand>, UnregisterStudentCommandHandler>();
        services.AddScoped<ICommandHandler<EditStudentPersonalInfoCommand>, EditStudentPersonalInfoCommandHandler>();
        services.AddScoped<ICommandHandler<EnrollStudentCommand>, EnrollStudentCommandHandler>();
        services.AddScoped<ICommandHandler<GradeStudentCommand>, GradeStudentCommandHandler>();
        services.AddScoped<ICommandHandler<TransferStudentCommand>, TransferStudentCommandHandler>();
        services.AddScoped<ICommandHandler<DisenrollStudentCommand>, DisenrollStudentCommandHandler>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentManager, StudentManager>();

        services.AddScoped<Messages>();

        return services;
    }
}
