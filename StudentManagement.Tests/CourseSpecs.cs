using FluentAssertions;
using StudentManagement.Domain.AggregatesModel.Courses;
using Xunit;

namespace StudentManagement.Tests;

public class CourseSpecs
{
    [Fact]
    public void AllCourses_contains_all_courses()
    {
        //ARRANGE
        Course[] allCourses =
        {
            Course.Calculus,
            Course.Chemistry,
            Course.English
        };

        //ASSERT
        Course.AllCourses.Should().BeEquivalentTo(allCourses);
    }

    [Fact]
    public void FromId_returns_correct_course()
    {
        //ARRANGE
        Course expectedCourse = Course.Calculus;

        //ACT
        Course course = Course.FromId(1L).Value;

        //ASSERT
        course.Should().Be(expectedCourse);
    }

    [Fact]
    public void Courses_have_different_ids()
    {
        Course.AllCourses.Should().OnlyHaveUniqueItems(x => x.Id);
    }
}
