using StudentManagement.Domain.Models.Students;
using StudentManagement.Infrastructure;
using StudentManagement.Infrastructure.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.CustomSchemaIds(x => GetNestedDisplayName(x));
});

// EFCore configuration... bye bye !
//builder.Services.AddDbContext<SchoolContext>(
//    options =>
//    {
//        options.UseSqlite(builder.Configuration.GetConnectionString("SchoolDb"));
//        options.EnableSensitiveDataLogging();
//        options.LogTo(Console.WriteLine);
//    });

// Dependency Injection
builder.Services.AddSingleton(new SessionFactory(builder.Configuration.GetConnectionString("SchoolDb")));
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



/**
 *  <summary>
 *  Generates a string containing the type name and all its
 *  parents in descending order and separated by periods
 *  </summary>
 *  <example>
 *  x = C
 *  returns
 *  "A.B.C"
 *  </example>
 */
string GetNestedDisplayName(Type x)
{
    if (x.IsNested && x.DeclaringType is not null)
    {
        return $"{GetNestedDisplayName(x.DeclaringType)}.{x.Name}";
    }
    else
    {
        return x.Name;
    }
}