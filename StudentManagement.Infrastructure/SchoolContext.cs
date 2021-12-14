using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using StudentManagement.Domain.Models.Students;

namespace StudentManagement.Infrastructure;

public sealed class SchoolContext : DbContext
{
    private static readonly Type[] EnumerationTypes = { typeof(Course) };

    private readonly string _connectionString;
    private readonly bool _useConsoleLogger;

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    public SchoolContext()
    {

    }

    public SchoolContext(string connectionString, bool useConsoleLogger)
    {
        _connectionString = connectionString;
        _useConsoleLogger = useConsoleLogger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
            .AddConsole();
        });

        if (_connectionString is null)
        {
            builder
                .UseSqlite();
        }
        else
        {
            builder
                .UseSqlite(_connectionString);
        }

        if (_useConsoleLogger)
        {
            builder
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(true);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Student>(x =>
        {
            x.ToTable("Student").HasKey(y => y.Id);

            // Single property value object
            x.Property(x => x.Email)
                .HasConversion(y => y.Value, y => Email.Create(y).Value);

            // Multi-property value object
            x.OwnsOne(y => y.Name, y =>
            {
                y.Property(z => z.First).HasColumnName("Firstname");
                y.Property(z => z.Last).HasColumnName("Lastname");
            });

            x.HasMany(x => x.Enrollments).WithOne(y => y.Student)
                .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field);

            x.HasMany(x => x.Disenrollments).WithOne(y => y.Student)
                .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Entity<Course>(x =>
        {
            x.ToTable("Course").HasKey(y => y.Id);

            x.Property(y => y.Name);

            foreach (var course in Course.AllCourses)
            {
                x.HasData(course);
            }
        });

        builder.Entity<Enrollment>(x =>
        {
            x.ToTable("Enrollment").HasKey(y => y.Id);

            x.HasOne(y => y.Student).WithMany(y => y.Enrollments);

            x.HasOne(y => y.Course).WithMany();

            x.Property(p => p.Grade);
        });

        builder.Entity<Disenrollment>(x =>
        {
            x.ToTable("Disenrollment").HasKey(y => y.Id);

            x.HasOne(y => y.Student).WithMany(y => y.Disenrollments);

            x.HasOne(y => y.Course).WithMany();

            x.Property(y => y.Comment);

            x.Property(y => y.DateTime);
        });
    }

    public override int SaveChanges()
    {
        IEnumerable<EntityEntry> enumerationEntries = ChangeTracker.Entries()
            .Where(x => EnumerationTypes.Contains(x.Entity.GetType()));

        foreach (EntityEntry enumerationEntry in enumerationEntries)
        {
            enumerationEntry.State = EntityState.Unchanged;
        }

        List<Entity> entities = ChangeTracker
            .Entries()
            .Where(x => x.Entity is Entity)
            .Select(x => (Entity)x.Entity)
            .ToList();

        return base.SaveChanges();
    }
}
