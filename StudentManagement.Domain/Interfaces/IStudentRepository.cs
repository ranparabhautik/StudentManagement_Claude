using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Interfaces;

/// <summary>
/// Repository contract specific to <see cref="Student"/> entities.
/// </summary>
public interface IStudentRepository : IGenericRepository<Student>
{
}
