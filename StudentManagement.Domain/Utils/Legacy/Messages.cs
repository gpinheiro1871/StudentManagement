using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Utils.Legacy;

public sealed class Messages
{
    private IServiceProvider _serviceProvider;

    public Messages(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<UnitResult<Error>> DispatchAsync(ICommand command)
    {
        Type type = typeof(ICommandHandler<>);
        Type[] typeArgs = { command.GetType() };
        Type handlerType = type.MakeGenericType(typeArgs);

        dynamic? handler = _serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new NullReferenceException();
        }

        UnitResult<Error> result = await handler.HandleAsync((dynamic)command);

        return result;
    }

    public async Task<Result<T, Error>> DispatchAsync<T>(IQuery<T> query)
    {

        Type type = typeof(IQueryHandler<,>);
        Type[] typeArgs = { query.GetType(), typeof(T) };
        Type handlerType = type.MakeGenericType(typeArgs);

        Console.WriteLine(handlerType.FullName);

        dynamic? handler = _serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new NullReferenceException();
        }

        Result<T, Error> result = await handler.HandleAsync((dynamic)query);

        return result;
    }
}
