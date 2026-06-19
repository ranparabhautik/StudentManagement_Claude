using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Interfaces;

/// <summary>
/// Application service contract for student management operations.
/// </summary>
public interface IStudentService
{
    /// <summary>Returns all students.</summary>
    /// <returns>A collection of <see cref="StudentResponseDto"/>.</returns>
    Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();

    /// <summary>Returns a single student by identifier.</summary>
    /// <param name="id">The student's unique identifier.</param>
    /// <returns>The matching <see cref="StudentResponseDto"/>, or <c>null</c> if not found.</returns>
    Task<StudentResponseDto?> GetStudentByIdAsync(int id);

    /// <summary>Creates and persists a new student.</summary>
    /// <param name="dto">Data required to create the student.</param>
    /// <returns>The newly created student as a <see cref="StudentResponseDto"/>.</returns>
    Task<StudentResponseDto> AddStudentAsync(CreateStudentDto dto);

    /// <summary>Updates an existing student's information.</summary>
    /// <param name="dto">Updated student data, including the target identifier.</param>
    /// <returns>The updated student as a <see cref="StudentResponseDto"/>.</returns>
    Task<StudentResponseDto> UpdateStudentAsync(UpdateStudentDto dto);

    /// <summary>Deletes a student by identifier.</summary>
    /// <param name="id">The unique identifier of the student to delete.</param>
    Task DeleteStudentAsync(int id);
}
