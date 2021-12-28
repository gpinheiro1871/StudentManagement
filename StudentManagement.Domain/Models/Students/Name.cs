using CSharpFunctionalExtensions;
using StudentManagement.Domain.Utils;
using static StudentManagement.Domain.Utils.Error;

namespace StudentManagement.Domain.Models.Students;

public class Name : ValueObject
{
    public string First { get; }
    public string Last { get; }

    #pragma warning disable CS8618 
    protected Name() { }

    private Name(string firstName, string lastName)
    {
        First = firstName;
        Last = lastName;
    }

    public static Result<Name, Error> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired();
        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired();

        firstName = firstName.Trim();
        lastName = lastName.Trim();

        if (firstName.Length > 200)
            return Errors.General.InvalidLength(firstName);
        if (lastName.Length > 200)
            return Errors.General.InvalidLength(lastName);

        return new Name(firstName, lastName);
    }

    public override string ToString()
    {
        return $"{First} {Last}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return First;
        yield return Last;
    }
}
