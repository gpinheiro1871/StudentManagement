using Xunit;
using FluentAssertions;
using StudentManagement.Domain.Models.Students;
using System;
using System.Threading.Tasks;

namespace StudentManagement.Tests;

public class StudentSpecs
{
    [Fact]
    public async Task New_student_is_enrolled_in_one_course_with_no_grade()
    {
        //ACT
        var student = await Builders.buildStudent();

        //ASSERT
        student.Enrollments.Should().HaveCount(1);
        student.Enrollments[0].Grade.Should().BeNull();
    }

    [Fact]
    public async Task Can_enroll_student()
    {
        //ARRANGE
        Student student = await Builders.buildStudent();
        Course course = Course.FromId(2L).Value;

        //ACT
        student.Enroll(course);

        //ASSERT
        student.Enrollments.Should().HaveCount(2);
        student.Enrollments[1].Course.Should().Be(course);
    }

    [Fact]
    public async Task Cannot_enroll_student_in_more_than_two_coursesAsync()
    {
        //ARRANGE
        Student student = await Builders.buildStudent();
        Course course = Course.FromId(2L).Value;
        Course course2 = Course.FromId(3L).Value;
        student.Enroll(course);

        //ACT
        Action action = () => student.Enroll(course2);

        //ASSERT
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task Cannot_enroll_student_in_the_same_course_twice()
    {
        //ARRANGE
        Student student = await Builders.buildStudent();
        Course course = Course.FromId(1L).Value;

        //ACT
        Action action = () => student.Enroll(course);

        //ASSERT
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task Can_grade_studentAsync()
    {
        //ARRANGE
        Student student = await Builders.buildStudent();
        Course course = Course.FromId(1L).Value;

        //ACT
        student.Grade(course, Grade.A);

        //ASSERT
        student.Enrollments[0].Grade.Should().Be(Grade.A);
    }

    [Fact]
    public async Task Disenrolling_student_produces_disenrollment_with_commentAsync()
    {
        //ARRANGE
        Student student = await Builders.buildStudent();
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
    public async Task Can_transfer_student()
    {
        //ARRANGE
        Student student = await Builders.buildStudent();
        Course course = Course.FromId(2L).Value;
        Grade grade = Grade.A;

        //ACT
        student.Transfer(1, course, grade);

        //ASSERT
        student.FirstEnrollment.Course.Should().Be(course);
        student.FirstEnrollment.Grade.Should().Be(grade);

    }
}