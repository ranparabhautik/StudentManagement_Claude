using AutoMapper;
using StudentManagement.Application.DTOs;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Mapping;

/// <summary>
/// AutoMapper profile defining mappings between <see cref="Student"/> entities and DTOs.
/// </summary>
public class StudentMappingProfile : Profile
{
    /// <summary>
    /// Registers all student-related entity-to-DTO and DTO-to-entity mappings.
    /// </summary>
    public StudentMappingProfile()
    {
        CreateMap<Student, StudentResponseDto>();
        CreateMap<CreateStudentDto, Student>();
        CreateMap<UpdateStudentDto, Student>();
    }
}
