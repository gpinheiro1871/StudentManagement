using CSharpFunctionalExtensions;
using StudentManagement.Domain.Utils;

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

    public static DomainResult<Name> Create(string firstName, string lastName)
    {
        List<Message> messages = new List<Message>();

        if (string.IsNullOrWhiteSpace(firstName))
            messages.Add(ErrorMessage.Create(nameof(firstName), "First name should not be empty"));
        if (string.IsNullOrWhiteSpace(lastName))
            messages.Add(ErrorMessage.Create(nameof(lastName), "Last name should not be empty"));

        firstName = firstName.Trim();
        lastName = lastName.Trim();

        if (firstName.Length > 200)
            messages.Add(ErrorMessage.Create(nameof(firstName), "First name is too long"));
        if (lastName.Length > 200)
            messages.Add(ErrorMessage.Create(nameof(lastName), "Last name is too long"));

        return DomainResult.FromSuccess(messages, new Name(firstName, lastName));
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
