using Xunit;
using FluentAssertions;
using StudentManagement.Domain.Models.Students;

namespace StudentManagement.Tests;

public class EmailSpecs
{

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Cannot_create_email_with_null_or_empty_strings(string email)
    {
        //ACT
        var emailResult = Email.Create(email);

        //ASSERT
        emailResult.IsFailure.Should().BeTrue();
        emailResult.Error.Should().NotBeNull();
    }

    [Fact]
    public void Cannot_create_invalid_email()
    {
        //ARRANGE
        var email = "johnmarston123$gmail.com";

        //ACT
        var emailResult = Email.Create(email);

        //ASSERT
        emailResult.IsFailure.Should().BeTrue();
        emailResult.Error.Should().NotBeNull();
    }

    [Fact]
    public void Can_create_valid_email()
    {
        //ARRANGE
        var email = "johnmarston123@gmail.com";

        //ACT
        var emailResult = Email.Create(email);

        //ASSERT
        emailResult.IsSuccess.Should().BeTrue();
        emailResult.Value.Value.Should().Be(email);
    }
}
