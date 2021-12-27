namespace StudentManagement.Domain.Utils;

public class DomainResult<T> : DomainActionResult
{

    private readonly T _value;
    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return _value;
        }
    }

    protected internal DomainResult(List<Message>? messages, T data)
        : base(messages)
    {
        if (IsSuccess)
        {
            _value = data;
        }
    }

    internal void Merge(DomainActionResult item)
    {
        _messages.AddRange(item.Messages);
    }
}

public class DomainResult
{
    public static DomainResult<T> Combine<T>(T finalData, params DomainActionResult[] results)
    {
        DomainResult<T> combinedResult = From(finalData);

        results.ToList().ForEach(combinedResult.Merge);

        return combinedResult;
    }

    internal static DomainResult<T> FromSuccess<T>(List<Message> messages, T data)
    {
        return new DomainResult<T>(messages, data);
    }

    internal static DomainResult<T> From<T>(T data)
    {
        return new DomainResult<T>(null, data);
    }
}