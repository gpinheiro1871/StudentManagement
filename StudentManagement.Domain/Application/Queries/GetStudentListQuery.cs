using Enrollment = StudentManagement.Domain.Application.Queries.Responses.GetStudentListResponse.Enrollment;
using Student = StudentManagement.Domain.Application.Queries.Responses.GetStudentListResponse.Student;
using StudentManagement.Domain.Application.Queries.Responses;
using StudentManagement.Domain.Infrastructure;
using MediatR;
using Dapper;

namespace StudentManagement.Domain.Application.Queries;

public class GetStudentListQuery : IRequest<GetStudentListResponse>
{
    internal sealed class GetStudentListQueryHandler
        : IRequestHandler<GetStudentListQuery, GetStudentListResponse>
    {
        private readonly DbSession _dbSession;

        public GetStudentListQueryHandler(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<GetStudentListResponse> Handle(GetStudentListQuery request,
            CancellationToken cancellationToken)
        {
            using var conn = _dbSession.Connection;

            var studentDictionary = new Dictionary<long, Student>();

            var students = (await conn.QueryAsync<Student, Enrollment, Student>(
                @"SELECT s.Id, (s.FirstName || ' ' || s.LastName) Name,
                s.Email, e.Grade, c.Id CourseId, c.Name CourseName FROM Student s
                INNER JOIN Enrollment e ON e.StudentId = s.Id
                INNER JOIN Course c ON c.Id = e.CourseId
                ORDER BY s.Id",
                (student, enrollment) =>
                {
                    if (!studentDictionary.TryGetValue(student.Id, out Student? studentEntry))
                    {
                        studentEntry = student;
                        studentEntry.Enrollments = new List<Enrollment>();

                        studentDictionary.Add(studentEntry.Id, studentEntry);
                    }

                    studentEntry?.Enrollments?.Add(enrollment);

                    return student;
                },
                splitOn: "Grade"))
                .DistinctBy(x => x.Id)
                .ToList();

            return new GetStudentListResponse() { Students = students };
        }
    }
}