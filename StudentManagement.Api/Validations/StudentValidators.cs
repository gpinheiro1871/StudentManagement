﻿using FluentValidation;
using StudentManagement.Api.DataContracts;
using StudentManagement.Api.Utils;
using StudentManagement.Domain.Models.Students;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Validations;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(IStudentRepository studentRepository)
    {
        RuleFor(x => new { x.FirstName, x.LastName })
            .MustBeValueObject(x => Name.Create(x.FirstName, x.LastName))
            .WithName("Name");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .MustBeValueObject(Email.Create)
            .MustAsync(async (x, cancellation) =>
            {
                return !await studentRepository.EmailExistsAsync(Email.Create(x).Value);
            })
            .WithMessage(Errors.Student.EmailIsTaken().Serialize());

        RuleFor(x => x.Enrollment).MustBeEntity(x => Course.FromId(x.CourseId));
    }
}

public class EnrollRequestValidator : AbstractValidator<EnrollRequest>
{
    public EnrollRequestValidator()
    {
        RuleFor(x => x.CourseId).MustBeEntity(x => Course.FromId(x));
    }
}

public class EditPersonalInfoRequestValidator : AbstractValidator<EditPersonalInfoRequest>
{
    public EditPersonalInfoRequestValidator()
    {
        RuleFor(x => new { x.FirstName, x.LastName })
            .MustBeValueObject(x => Name.Create(x.FirstName, x.LastName))
            .WithName("Name");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .MustBeValueObject(Email.Create);
    }
}

public class GradeRequestValidator : AbstractValidator<GradeRequest>
{
    public GradeRequestValidator()
    {
        RuleFor(x => x.CourseId).MustBeEntity(x => Course.FromId(x));
        
        RuleFor(x => x.Grade)
            .IsInEnum()
            .WithMessage(Errors.General.ValueIsInvalid(nameof(Grade)).Serialize());
    }
}

public class TransferRequestValidator : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.CourseId).MustBeEntity(x => Course.FromId(x));

        When(x => x.Grade is not null, () =>
        {
            RuleFor(x => x.Grade)
                .IsInEnum()
                .WithMessage(Errors.General.ValueIsInvalid(nameof(Grade)).Serialize());
        });
    }
}

public class DisenrollRequestValidator : AbstractValidator<DisenrollRequest>
{
    public DisenrollRequestValidator()
    {
        RuleFor(x => x.CourseId).MustBeEntity(x => Course.FromId(x));

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage(Errors.General.InvalidLength(nameof(DisenrollRequest.Comment)).Serialize());
    }
}