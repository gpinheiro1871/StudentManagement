using FluentNHibernate.Mapping;
using StudentManagement.Domain.AggregatesModel.Students;

namespace StudentManagement.Domain.Infrastructure.Mappings;

public class StudentMap : ClassMap<Student>
{
    public StudentMap()
    {
        Id(x => x.Id);

        Component(x => x.Name, x =>
        {
            x.Map(y => y.FirstName).Column("FirstName");
            x.Map(y => y.LastName).Column("LastName");
        });

        Map(x => x.Email)
            .CustomType<string>().Access.CamelCaseField(Prefix.Underscore);

        HasMany(x => x.Enrollments).Access.CamelCaseField(Prefix.Underscore).Inverse().Cascade.AllDeleteOrphan();
        HasMany(x => x.Disenrollments).Access.CamelCaseField(Prefix.Underscore).Inverse().Cascade.AllDeleteOrphan();
    }
}

public class EnrollmentMap : ClassMap<Enrollment>
{
    public EnrollmentMap()
    {
        Id(x => x.Id);

        Map(x => x.Grade).CustomType<Grade>();

        References(x => x.Student);
        References(x => x.Course);
    }
}

public class DisenrollmentMap : ClassMap<Disenrollment>
{
    public DisenrollmentMap()
    {
        Id(x => x.Id);

        Map(x => x.DateTime);
        Map(x => x.Comment);

        References(x => x.Student);
        References(x => x.Course);
    }
}