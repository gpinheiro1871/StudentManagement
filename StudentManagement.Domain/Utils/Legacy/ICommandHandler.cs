using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Utils.Legacy;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<UnitResult<Error>> HandleAsync(TCommand command);
}
