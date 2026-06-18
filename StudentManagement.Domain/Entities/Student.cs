using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Domain.Entities;

public class Student : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public string Grade { get; set; } = string.Empty;
}
