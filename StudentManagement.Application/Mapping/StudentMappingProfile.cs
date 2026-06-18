using AutoMapper;
using StudentManagement.Application.DTOs;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Mapping;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, StudentResponseDto>();
        CreateMap<CreateStudentDto, Student>();
        CreateMap<UpdateStudentDto, Student>();
    }
}
