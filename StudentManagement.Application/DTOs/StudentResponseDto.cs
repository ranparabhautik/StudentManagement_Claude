namespace StudentManagement.Application.DTOs;

public class StudentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public string Grade { get; set; } = string.Empty;
}
