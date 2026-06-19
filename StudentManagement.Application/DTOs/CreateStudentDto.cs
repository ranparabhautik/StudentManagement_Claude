namespace StudentManagement.Application.DTOs;

/// <summary>
/// Data transfer object for creating a new student.
/// </summary>
public class CreateStudentDto
{
    /// <summary>Gets or sets the student's full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the date the student enrolled.</summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>Gets or sets the student's grade (e.g. A, B+, C).</summary>
    public string Grade { get; set; } = string.Empty;
}
