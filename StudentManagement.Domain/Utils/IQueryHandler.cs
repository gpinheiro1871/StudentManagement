using CSharpFunctionalExtensions;

namespace StudentManagement.Domain.Utils
{
    public interface IQueryHandler<TQuery, TResult> 
        where TQuery : IQuery<TResult>
    {
        Task<Result<TResult, Error>> HandleAsync(TQuery query);
    }
}
