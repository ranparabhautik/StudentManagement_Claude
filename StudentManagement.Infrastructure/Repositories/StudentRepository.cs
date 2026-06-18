using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Infrastructure.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
}
