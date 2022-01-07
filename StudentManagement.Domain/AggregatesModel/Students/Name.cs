using CSharpFunctionalExtensions;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.AggregatesModel.Students;

public class Name : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

#pragma warning disable CS8618
    protected Name() { }

    private Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<Name, Error> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired(nameof(FirstName));
        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired(nameof(LastName));

        firstName = firstName.Trim();
        lastName = lastName.Trim();

        if (firstName.Length > 200)
            return Errors.General.InvalidLength(nameof(FirstName));
        if (lastName.Length > 200)
            return Errors.General.InvalidLength(nameof(LastName));

        return new Name(firstName, lastName);
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
