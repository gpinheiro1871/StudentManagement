using Xunit;
using FluentAssertions;
using StudentManagement.Domain.Models.Students;
using System;

namespace StudentManagement.Tests;

public class EnrollmentSpecs
{
    [Fact]
    public void Cannot_create_enrollment_with_invalid_student()
    {
        //ARRANGE
        Course course = Course.English;

        //ACT
        Action action = () => new Enrollment(null, course, null);

        //ASSERT
        action.Should().Throw<ArgumentNullException>();
    } 
    
    [Fact]
    public void Cannot_create_enrollment_with_invalid_course()
    {
        //ARRANGE
        Student student = Builders.buildStudent();

        //ACT
        Action action = () => new Enrollment(student, null, null);

        //ASSERT
        action.Should().Throw<ArgumentNullException>();
    }
}
