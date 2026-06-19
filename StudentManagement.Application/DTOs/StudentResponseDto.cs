namespace StudentManagement.Application.DTOs;

/// <summary>
/// Data transfer object returned after student read/write operations.
/// </summary>
public class StudentResponseDto
{
    /// <summary>Gets or sets the student's unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the student's full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the date the student enrolled.</summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>Gets or sets the student's grade.</summary>
    public string Grade { get; set; } = string.Empty;
}
