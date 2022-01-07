using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Utils;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<UnitResult<Error>> HandleAsync(TCommand command);
}
