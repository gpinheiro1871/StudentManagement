using Disenrollment = StudentManagement.Domain.Application.Queries.Responses.GetStudentQueryResponse.Disenrollment;
using Enrollment = StudentManagement.Domain.Application.Queries.Responses.GetStudentQueryResponse.Enrollment;
using Student = StudentManagement.Domain.Application.Queries.Responses.GetStudentQueryResponse;
using StudentManagement.Domain.Infrastructure;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;
using Dapper;

namespace StudentManagement.Domain.Application.Queries;

public class GetStudentQuery : IRequest<Result<Student, Error>>
{
    public long StudentId { get; }

    public GetStudentQuery(long id)
    {
        StudentId = id;
    }

    internal sealed class GetStudentQueryHandler
        : IRequestHandler<GetStudentQuery, Result<Student, Error>>
    {
        private readonly DbSession _dbSession;

        public GetStudentQueryHandler(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<Result<Student, Error>> Handle(GetStudentQuery request,
            CancellationToken cancellationToken)
        {
            using var conn = _dbSession.Connection;

            string query =
                @"SELECT * FROM Student s
                WHERE s.Id = @StudentId;

                SELECT *, c.Name CourseName FROM Enrollment
                INNER JOIN Course c ON c.Id = CourseId
                WHERE StudentId = @StudentId;

                SELECT *, c.Name CourseName FROM Disenrollment
                INNER JOIN Course c ON c.Id = CourseId
                WHERE StudentId = @StudentId;";

            using var multi = await conn.QueryMultipleAsync(query, param: request);

            Student student = await multi.ReadFirstOrDefaultAsync<Student>();

            if (student is null)
            {
                return Errors.General.NotFound(request.StudentId);
            }

            List<Enrollment> enrollments = (await multi.ReadAsync<Enrollment>()).ToList();
            List<Disenrollment> disenrollments = (await multi.ReadAsync<Disenrollment>()).ToList();

            student.FirstEnrollment = enrollments.ElementAtOrDefault(0);
            student.SecondEnrollment = enrollments.ElementAtOrDefault(1);

            student.Disenrollments = disenrollments;

            return student;
        }
    }
}