using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Dtos;
using StudentManagement.Domain.Models.Students;
using StudentManagement.Domain.Services;
using StudentManagement.Domain.Utils;
using StudentManagement.Infrastructure;

namespace StudentManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class StudentsController : ControllerBase
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
    [ProducesResponseType(typeof(List<StudentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        IReadOnlyCollection<Student> students = await _studentRepository.QueryAll();

        List <StudentDto> dtos = students.Select(x => ConvertToDto(x)).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(long id)
    {
        Student? student = await _studentRepository.QueryByIdAsync(id);

        if (student is null)
        {
            return NotFound();
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
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(RegisterRequest dto)
    {
        //find course
        Course? course = Course.FromId(dto.Enrollment.CourseId);
        if (course is null)
        {
            return BadRequest("Course not found.");
        }

        //create student
        Name name = Name.Create(dto.FirstName, dto.LastName).Value;

        Email email = Email.Create(dto.Email).Value;

        var studentResult = await _studentManager.Create(name, email, course);

        if (studentResult.IsFailure)
        {
            return Ok(studentResult.Error);
        }

        Student student = studentResult.Value;

        await _studentRepository.SaveAsync(student);

        await _unitOfWork.CommitAsync();

        //return dto
        return Ok(ConvertToDto(student));
    }

    // Unregister
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Unregister(long id)
    {
        //Find student
        Student? student = await _studentRepository.GetByIdAsync(id);

        if (student is null)
        {
            return BadRequest();
        }

        await _studentRepository.DeleteAsync(student);

        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    // EditPersonalInfo
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> EditPersonalInfo(long id, EditPersonalInfoRequest dto)
    {
        //Find student
        Student? student = await _studentRepository.GetByIdAsync(id);

        if (student is null)
        {
            return NotFound();
        }

        Name name = Name.Create(dto.FirstName, dto.LastName).Value;

        Email email = Email.Create(dto.Email).Value;

        UnitResult<Error> result = await _studentManager.EditPersonalInfo(student, name, email);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    // Enroll
    [HttpPost("{id}/enrollments")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Enroll(long id, EnrollRequest dto)
    {
        //find course
        Course? course = Course.FromId(dto.CourseId);

        //find student
        Student? student = await _studentRepository.GetByIdAsync(id);

        if (course is null || student is null)
        {
            return BadRequest();
        }

        student.Enroll(course);

        await _unitOfWork.CommitAsync();

        return Ok(ConvertToDto(student));
    }

    // Grade
    [HttpPut("{id}/enrollments")]
    public async Task<IActionResult> Grade(long id, GradeRequest dto)
    {
        //find course
        Course? course = Course.FromId(dto.CourseId);

        //find student
        Student? student = await _studentRepository.GetByIdAsync(id);

        if (course is null || student is null)
        {
            return BadRequest();
        }

        student.Grade(course, dto.Grade);

        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    //Transfer
    [HttpPut("{id}/enrollments/{enrollmentNumber}")]
    public async Task<IActionResult> Transfer(long id, int enrollmentNumber, TransferRequest dto)
    {
        //find course
        Course? course = Course.FromId(dto.CourseId);

        //find student
        Student? student = await _studentRepository.GetByIdAsync(id);

        if (course is null || student is null)
        {
            return BadRequest();
        }

        student.Transfer(enrollmentNumber, course, dto.Grade);

        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    // Disenroll
    [HttpDelete("{id}/enrollments")]
    public async Task<IActionResult> Disenroll(long id, DisenrollRequest dto)
    {
        //find course
        Course? course = Course.FromId(dto.CourseId);

        //find student
        Student? student = await _studentRepository.GetByIdAsync(id);

        //Course? course = await _schoolContext.Courses.FindAsync(dto.CourseId);

        if (course is null || student is null)
        {
            return BadRequest();
        }

        student.Disenroll(course, dto.Comment);

        await _unitOfWork.CommitAsync();

        return NoContent();
    }
}
