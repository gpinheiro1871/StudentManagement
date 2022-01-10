using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DataContracts;
using StudentManagement.Api.Utils;
using StudentManagement.Domain.AggregatesModel.Students;
using StudentManagement.Domain.Application.Commands;
using StudentManagement.Domain.Application.Dtos;
using StudentManagement.Domain.Application.Queries;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class StudentsController : ApplicationController
{
    private readonly Messages _messages;

    public StudentsController(Messages messages)
        : base(messages)
    {
        _messages = messages;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Envelope<List<StudentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() =>
        await FromQuery(nameof(Student),
            new GetStudentListQuery());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Envelope<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long id) =>
        await FromQuery(nameof(Student),
            new GetStudentQuery(id));

    // Register
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto) =>
        await FromCommand(nameof(Student), new RegisterStudentCommand(
            dto.FirstName, dto.LastName, dto.Email, dto.Enrollment.CourseId));

    // Unregister
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Unregister(long id) => 
        await FromCommand(nameof(Student), 
            new UnregisterStudentCommand(id));

    // EditPersonalInfo
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditPersonalInfo(long id, EditPersonalInfoRequest dto) => 
        await FromCommand(nameof(Student), 
            new EditStudentPersonalInfoCommand(id, dto.FirstName, dto.LastName, dto.Email));

    // Enroll
    [HttpPost("{id}/enrollments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Enroll(long id, EnrollRequest dto) => 
        await FromCommand(nameof(Student), 
            new EnrollStudentCommand(id, dto.CourseId));

    // Grade
    [HttpPut("{id}/enrollments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Grade(long id, GradeRequest dto) => 
        await FromCommand(nameof(Student), 
            new GradeStudentCommand(id, dto.CourseId, dto.Grade));

    //Transfer
    [HttpPut("{id}/enrollments/{enrollmentNumber}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Transfer(long id, int enrollmentNumber, TransferRequest dto) =>
        await FromCommand(nameof(Student), 
            new TransferStudentCommand(id, enrollmentNumber, dto.CourseId, dto.Grade));

    // Disenroll
    [HttpDelete("{id}/enrollments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Disenroll(long id, DisenrollRequest dto) =>
        await FromCommand(nameof(Student), 
            new DisenrollStudentCommand(id, dto.CourseId, dto.Comment));
}
