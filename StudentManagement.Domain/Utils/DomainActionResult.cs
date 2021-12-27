namespace StudentManagement.Domain.Utils;

public class DomainActionResult
{
    protected readonly List<Message> _messages;
    public List<Message> Messages => _messages;

    public bool IsFailure => Messages.Any(x => x is ErrorMessage);
    public bool IsSuccess => !IsFailure;

    protected DomainActionResult(List<Message>? messages)
    {
        _messages = messages ?? new();
    }

    internal static DomainActionResult From(List<Message> messages)
    {
        return new DomainActionResult(messages);
    }
}