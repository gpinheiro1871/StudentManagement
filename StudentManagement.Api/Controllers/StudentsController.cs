using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DataContracts;
using StudentManagement.Api.Utils;
using StudentManagement.Domain.Models.Students;
using StudentManagement.Domain.Services;
using StudentManagement.Domain.Utils;
using StudentManagement.Infrastructure;

namespace StudentManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class StudentsController : ApplicationController
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentManager _studentManager;

    public StudentsController(UnitOfWork unitOfWork, 
        IStudentRepository studentRepository, IStudentManager studentManager)
    {
        _unitOfWork = unitOfWork;
        _studentRepository = studentRepository;
        _studentManager = studentManager;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Envelope<List<StudentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        IReadOnlyCollection<Student> students = await _studentRepository.QueryAllAsync();

        List <StudentDto> dtos = students.Select(x => ConvertToDto(x)).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Envelope<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long id)
    {
        Student? student = await _studentRepository.QueryByIdAsync(id);

        if (student is null)
        {
            return NotFound(nameof(Student), Errors.General.NotFound(id));
        }

        return Ok(ConvertToDto(student));
    }

    private StudentDto ConvertToDto(Student student)
    {
        var firstEnrollment = student.FirstEnrollment is null
            ? null
            : new StudentDto.EnrollmentDto()
            {
                CourseId = student.FirstEnrollment.Course.Id,
                CourseName = student.FirstEnrollment.Course.Name,
                Grade = student.FirstEnrollment.Grade?.ToString()
            };
        
        var secondEnrollment = student.SecondEnrollment is null
            ? null
            : new StudentDto.EnrollmentDto()
            {
                CourseId = student.SecondEnrollment.Course.Id,
                CourseName = student.SecondEnrollment.Course.Name,
                Grade = student.SecondEnrollment.Grade?.ToString()
            };

        return new StudentDto
        {
            Id = student.Id,
            Name = student.Name.ToString(),
            Email = student.Email.ToString(),
            FirstEnrollment = firstEnrollment,
            SecondEnrollment = secondEnrollment
        };
    }

    // Register
    [HttpPost]
    [ProducesResponseType(typeof(Envelope<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequest dto)
    {
        Course course = Course.FromId(dto.Enrollment.CourseId).Value;

        Name name = Name.Create(dto.FirstName, dto.LastName).Value;

        Email email = Email.Create(dto.Email).Value;

        Student student = await _studentManager.CreateAsync(name, email, course);

        await _studentRepository.SaveAsync(student);

        await _unitOfWork.CommitAsync();

        return CreatedAtAction(
            nameof(Get), nameof(StudentsController), new { id = student.Id }, student);
    }

    // Unregister
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unregister(long id)
    {
        //Find student
        Student? student = await _studentRepository.GetByIdAsync(id);

        if (student is null)
        {
            return NotFound(nameof(Student), Errors.General.NotFound(id));
        }

        await _studentRepository.DeleteAsync(student);

        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    // EditPersonalInfo
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditPersonalInfo(long id, EditPersonalInfoRequest dto)
    {
        //Find student
        Student? student = await _studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(nameof(Student), Errors.General.NotFound(id));
        }

        Name name = Name.Create(dto.FirstName, dto.LastName).Value;

        Email email = Email.Create(dto.Email).Value;

        // Check for email uniqueness
        if (email != student.Email)
        {
            bool emailExists = await _studentRepository.EmailExistsAsync(email);

            if (emailExists)
            {
                return Error(nameof(dto.Email), Errors.Student.EmailIsTaken());
            }
        }

        await _studentManager.EditPersonalInfoAsync(student, name, email);

        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    // Enroll
    [HttpPost("{id}/enrollments")]
    [ProducesResponseType(typeof(Envelope<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Enroll(long id, EnrollRequest dto)
    {
        Student? student = await _studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(nameof(id), Errors.General.NotFound(id));
        }

        Course course = Course.FromId(dto.CourseId).Value;

        var canEnroll = student.CanEnroll(course);

        if (canEnroll.IsFailure)
        {
            return Error(nameof(Student), canEnroll.Error);
        }

        student.Enroll(course);

        await _unitOfWork.CommitAsync();

        return Ok(ConvertToDto(student));
    }

    // Grade
    [HttpPut("{id}/enrollments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Grade(long id, [FromBody] GradeRequest dto)
    {
        Student? student = await _studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(nameof(id), Errors.General.NotFound(id));
        }

        Course course = Course.FromId(dto.CourseId).Value;

        var result = student.Grade(course, dto.Grade);
        if (result.IsFailure)
        {
            return Error(nameof(Student), result.Error);
        }

        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    //Transfer
    [HttpPut("{id}/enrollments/{enrollmentNumber}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Transfer(long id, int enrollmentNumber, TransferRequest dto)
    {
        Student? student = await _studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(nameof(id), Errors.General.NotFound(id));
        }

        Course course = Course.FromId(dto.CourseId).Value;

        var result = student.Transfer(enrollmentNumber, course, dto.Grade);
        if (result.IsFailure)
        {
            return Error(nameof(Student), result.Error);
        }

        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    // Disenroll
    [HttpDelete("{id}/enrollments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Disenroll(long id, DisenrollRequest dto)
    {
        //find student
        Student? student = await _studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(nameof(id), Errors.General.NotFound(id));
        }

        Course course = Course.FromId(dto.CourseId).Value;

        var result = student.Disenroll(course, dto.Comment);
        if (result.IsFailure)
        {
            return Error(nameof(Student), result.Error);
        }

        await _unitOfWork.CommitAsync();

        return NoContent();
    }
}
