using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Domain.Entities;

/// <summary>
/// Represents a student in the management system.
/// </summary>
public class Student : IEntity
{
    /// <summary>Gets or sets the unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the student's full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the date the student enrolled.</summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>Gets or sets the student's current grade (e.g. A, B+, C).</summary>
    public string Grade { get; set; } = string.Empty;
}
