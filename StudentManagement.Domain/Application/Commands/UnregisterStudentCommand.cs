using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace StudentManagement.Domain.Application.Commands;

public class UnregisterStudentCommand : IRequest<UnitResult<Error>>
{
    public long Id { get; }

    public UnregisterStudentCommand(long id)
    {
        Id = id;
    }

    internal sealed class UnregisterStudentCommandHandler
        : IRequestHandler<UnregisterStudentCommand, UnitResult<Error>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;

        public UnregisterStudentCommandHandler(IUnitOfWork unitOfWork,
            IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
        }

        public async Task<UnitResult<Error>> Handle(UnregisterStudentCommand request, 
            CancellationToken cancellationToken)
        {
            Student? student = await _studentRepository.GetByIdAsync(request.Id);

            if (student is null)
            {
                return Errors.General.NotFound(request.Id);
            }

            await _studentRepository.DeleteAsync(student);

            await _unitOfWork.CommitAsync();

            return UnitResult.Success<Error>();
        }
    }
}
