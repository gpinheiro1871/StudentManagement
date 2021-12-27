using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Utils;

public class ApiResponse<T>
{
    public T? Data { get; }

    private readonly List<Message> _messages;
    public IReadOnlyList<Message> Messages => _messages;
    public bool Success => !Messages.Any(x => x is ErrorMessage);
    public DateTime TimeGenerated { get; }

    protected internal ApiResponse(T? data, List<Message>? messages)
    {
        Data = data;

        _messages = messages ?? new();

        TimeGenerated = DateTime.UtcNow;
    }

    public void AddErrorMessage(string title, string body)
    {
        _messages.Add(ErrorMessage.Create(title, body));
    }  
    
    public void AddMessage(string title, string body)
    {
        _messages.Add(Message.Create(title, body));
    }
}

public sealed class ApiResponse : ApiResponse<string>
{
    public ApiResponse(List<Message>? messages)
        : base(null, messages)
    {
    }

    public static ApiResponse<T> From<T>(T? data)
    {
        return new ApiResponse<T>(data, null);
    }

    public static ApiResponse<T> From<T>(T? data, List<Message>? messages)
    {
        return new ApiResponse<T>(data, messages);
    } 
    
    public static ApiResponse<T> From<T>(DomainResult<T> domainResult)
    {
        return new ApiResponse<T>(domainResult.Value, domainResult.Messages);
    }

    public static ApiResponse From(DomainActionResult domainActionResult)
    {
        return new ApiResponse(domainActionResult.Messages);
    }
}