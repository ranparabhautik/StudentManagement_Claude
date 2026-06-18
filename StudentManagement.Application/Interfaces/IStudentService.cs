using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();
    Task<StudentResponseDto?> GetStudentByIdAsync(int id);
    Task<StudentResponseDto> AddStudentAsync(CreateStudentDto dto);
    Task<StudentResponseDto> UpdateStudentAsync(UpdateStudentDto dto);
    Task DeleteStudentAsync(int id);
}
