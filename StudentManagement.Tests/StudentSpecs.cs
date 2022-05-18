using Xunit;
using FluentAssertions;
using System;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.AggregatesModel.Courses;
using System.Linq;

namespace StudentManagement.Tests;

public class StudentSpecs
{
    [Fact]
    public void New_student_is_enrolled_in_one_course_with_no_grade()
    {
        //ACT
        var student = Build.Student();

        //ASSERT
        student.Enrollments.Should().HaveCount(1);
        student.Enrollments[0].Grade.Should().BeNull();
    }

    [Fact]
    public void Can_enroll_student()
    {
        //ARRANGE
        Student student = Build.Student();
        Course course = Course.FromId(2L).Value;

        //ACT
        student.Enroll(course);

        //ASSERT
        student.Enrollments.Should().HaveCount(2);
        student.Enrollments[1].Course.Should().Be(course);
    }

    [Fact]
    public void Cannot_enroll_student_in_more_than_two_courses()
    {
        //ARRANGE
        Student student = Build.Student();
        Course course = Course.FromId(2L).Value;
        Course course2 = Course.FromId(3L).Value;
        student.Enroll(course);

        //ACT
        Action action = () => student.Enroll(course2);

        //ASSERT
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Cannot_enroll_student_in_the_same_course_twice()
    {
        //ARRANGE
        Student student = Build.Student();
        Course course = Course.FromId(1L).Value;

        //ACT
        Action action = () => student.Enroll(course);

        //ASSERT
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Can_grade_student()
    {
        //ARRANGE
        Student student = Build.Student();
        Course course = Course.FromId(1L).Value;

        //ACT
        student.Grade(course, Grade.A);

        //ASSERT
        student.Enrollments[0].Grade.Should().Be(Grade.A);
    }

    [Fact]
    public void Disenrolling_student_produces_disenrollment_with_comment()
    {
        //ARRANGE
        Student student = Build.Student();
        Course course = Course.FromId(1L).Value;
        string comment = "Too bad!";

        //ACT
        student.Disenroll(course, comment);

        //ASSERT
        student.Enrollments.Should().HaveCount(0);
        student.Disenrollments.Should().HaveCount(1);
        student.Disenrollments[0].Comment.Should().Be(comment);
    }

    [Fact]
    public void Can_transfer_student()
    {
        //ARRANGE
        Student student = Build.Student();
        Course course = Course.FromId(2L).Value;
        Grade grade = Grade.A;

        //ACT
        student.Transfer(1, course, grade);

        //ASSERT
        student.Enrollments.ElementAtOrDefault(0).Should().NotBeNull();
        student.Enrollments.ElementAtOrDefault(0)?.Course.Should().Be(course);
        student.Enrollments.ElementAtOrDefault(0)?.Grade.Should().Be(grade);
    }
}
