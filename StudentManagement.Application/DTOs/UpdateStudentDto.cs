namespace StudentManagement.Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing student's information.
/// </summary>
public class UpdateStudentDto
{
    /// <summary>Gets or sets the unique identifier of the student to update.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the student's updated full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's updated email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's updated enrollment date.</summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>Gets or sets the student's updated grade (e.g. A, B+, C).</summary>
    public string Grade { get; set; } = string.Empty;
}
