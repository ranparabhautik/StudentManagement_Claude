using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Infrastructure.Repositories;

/// <summary>
/// In-memory repository for <see cref="Student"/> entities.
/// Inherits all CRUD operations from <see cref="GenericRepository{T}"/>.
/// </summary>
public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
}
