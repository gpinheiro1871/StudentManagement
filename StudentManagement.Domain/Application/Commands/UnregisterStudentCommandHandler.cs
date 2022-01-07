using CSharpFunctionalExtensions;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Domain.Application.Commands;

public class UnregisterStudentCommandHandler 
    : ICommandHandler<UnregisterStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentRepository _studentRepository;

    public UnregisterStudentCommandHandler(IUnitOfWork unitOfWork, 
        IStudentRepository studentRepository)
    {
        _unitOfWork = unitOfWork;
        _studentRepository = studentRepository;
    }

    public async Task<UnitResult<Error>> HandleAsync(UnregisterStudentCommand command)
    {
        //Find student
        Student? student = await _studentRepository.GetByIdAsync(command.Id);

        if (student is null)
        {
            return Errors.General.NotFound(command.Id);
        }

        await _studentRepository.DeleteAsync(student);

        await _unitOfWork.CommitAsync();

        return UnitResult.Success<Error>();
    }
}
