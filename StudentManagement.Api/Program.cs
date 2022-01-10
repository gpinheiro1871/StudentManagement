using StudentManagement.Api.DataContracts.Validations;
using StudentManagement.Domain.Application.Commands;
using StudentManagement.Api.Utils;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new NullableStringEnumConverter());
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
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
    config.CustomSchemaIds(x => Dependencies.GetNestedDisplayName(x));
    config.SchemaGeneratorOptions.UseAllOfForInheritance = true;
});
builder.Services.AddSwaggerGenNewtonsoftSupport ();

// Dependency Injection
builder.Services.AddNHibernate(builder.Configuration.GetConnectionString("SchoolDb"));
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(typeof(RegisterStudentCommand));

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