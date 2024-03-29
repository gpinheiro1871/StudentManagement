﻿using Xunit;
using FluentAssertions;
using StudentManagement.Domain.AggregatesModel.Students;

namespace StudentManagement.Tests
{
    public class NameSpecs
    {

        [Theory]
        [InlineData("", "Marston")]
        [InlineData("John", "")]
        [InlineData(" ", "Marston")]
        [InlineData("John", " ")]
        [InlineData(null, "Marston")]
        [InlineData("John", null)]
        public void Cannot_create_name_with_null_or_empty_strings(string first, string last)
        {
            //ACT
            var name = Name.Create(first, last);

            //ASSERT
            name.IsFailure.Should().BeTrue();
            name.Error.Should().NotBeNull();
        }

        [Fact]
        public void Can_create_name_with_valid_strings()
        {
            //ARRANGE
            var firstName = "John";
            var lastName = "Marston";

            //ACT
            var name = Name.Create("John", "Marston");

            //ASSERT
            name.IsSuccess.Should().BeTrue();
            name.Value.FirstName.Should().Be(firstName);
            name.Value.LastName.Should().Be(lastName);
        }
    }
}
