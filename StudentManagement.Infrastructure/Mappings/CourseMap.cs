using FluentNHibernate.Mapping;
using StudentManagement.Domain.Models.Students;

namespace StudentManagement.Infrastructure.Mappings
{
    public class CourseMap : ClassMap<Course>
    {
        public CourseMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
