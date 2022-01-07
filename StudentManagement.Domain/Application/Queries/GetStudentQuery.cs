using StudentManagement.Domain.Application.Dtos;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Queries;

public class GetStudentQuery : IQuery<StudentDto>
{
    public long Id { get; }

    public GetStudentQuery(long id)
    {
        Id = id;
    }
}
