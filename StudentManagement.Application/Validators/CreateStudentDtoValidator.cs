using FluentValidation;
using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Validators;

public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.EnrollmentDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Grade).NotEmpty().MaximumLength(5);
    }
}
