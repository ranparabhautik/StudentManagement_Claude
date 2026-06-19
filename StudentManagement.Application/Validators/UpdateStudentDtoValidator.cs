using FluentValidation;
using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Validators;

/// <summary>
/// Validates <see cref="UpdateStudentDto"/> input before student updates.
/// </summary>
public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
{
    /// <summary>
    /// Configures validation rules: positive identifier, non-empty name (max 100),
    /// valid email, enrollment date not in the future, and non-empty grade (max 5 chars).
    /// </summary>
    public UpdateStudentDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.EnrollmentDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Grade).NotEmpty().MaximumLength(5);
    }
}
