using FluentValidation.AspNetCore;
using StudentManagement.Api.Utils;
using StudentManagement.Api.Validations;
using StudentManagement.Domain.Models.Students;
using StudentManagement.Domain.Services;
using StudentManagement.Infrastructure;
using StudentManagement.Infrastructure.Repositories;
using System.ComponentModel;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(null, true));
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
    })
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.CustomSchemaIds(x => Helpers.GetNestedDisplayName(x));
    config.SchemaGeneratorOptions.UseAllOfForInheritance = true;
});

// Dependency Injection
builder.Services.AddSingleton(new SessionFactory(builder.Configuration.GetConnectionString("SchoolDb")));
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentManager, StudentManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/error-local-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();