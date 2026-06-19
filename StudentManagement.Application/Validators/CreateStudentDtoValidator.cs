using FluentValidation;
using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Validators;

/// <summary>
/// Validates <see cref="CreateStudentDto"/> input before student creation.
/// </summary>
public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
    /// <summary>
    /// Configures validation rules: non-empty name (max 100), valid email,
    /// enrollment date not in the future, and non-empty grade (max 5 chars).
    /// </summary>
    public CreateStudentDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.EnrollmentDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Grade).NotEmpty().MaximumLength(5);
    }
}
