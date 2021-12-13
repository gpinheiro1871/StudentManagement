using Xunit;
using FluentAssertions;
using StudentManagement.Domain.Models.Students;
using System;

namespace StudentManagement.Tests;

public class DisenrollmentSpecs
{
    [Fact]
    public void Cannot_create_disenrollment_with_invalid_student()
    {
        //ARRANGE
        Course course = Course.English;

        //ACT
        Action action = () => new Disenrollment(null, course, "a");

        //ASSERT
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Cannot_create_disenrollment_with_invalid_course()
    {
        //ARRANGE
        Student student = Builders.buildStudent();

        //ACT
        Action action = () => new Disenrollment(student, null, "a");

        //ASSERT
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Cannot_create_disenrollment_with_invalid_comment()
    {
        //ARRANGE
        Student student = Builders.buildStudent();
        Course course = Course.English;

        //ACT
        Action action = () => new Disenrollment(student, course, null);

        //ASSERT
        action.Should().Throw<ArgumentNullException>();
    }
}
