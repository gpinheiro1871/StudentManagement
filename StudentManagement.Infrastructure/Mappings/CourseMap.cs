﻿using FluentNHibernate.Mapping;
using StudentManagement.Domain.AggregatesModel.Courses;

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
