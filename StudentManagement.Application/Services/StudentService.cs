using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateStudentDto> _createValidator;
    private readonly IValidator<UpdateStudentDto> _updateValidator;
    private readonly ILogger<StudentService> _logger;

    public StudentService(
        IStudentRepository repository,
        IMapper mapper,
        IValidator<CreateStudentDto> createValidator,
        IValidator<UpdateStudentDto> updateValidator,
        ILogger<StudentService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
    {
        var students = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentResponseDto>>(students);
    }

    public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);
        return student is null ? null : _mapper.Map<StudentResponseDto>(student);
    }

    public async Task<StudentResponseDto> AddStudentAsync(CreateStudentDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var student = _mapper.Map<Student>(dto);
        await _repository.AddAsync(student);
        _logger.LogInformation("Student added with Id {StudentId}.", student.Id);
        return _mapper.Map<StudentResponseDto>(student);
    }

    public async Task<StudentResponseDto> UpdateStudentAsync(UpdateStudentDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var existing = await _repository.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Student with Id {dto.Id} not found.");

        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing);
        _logger.LogInformation("Student updated with Id {StudentId}.", existing.Id);
        return _mapper.Map<StudentResponseDto>(existing);
    }

    public async Task DeleteStudentAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Student with Id {id} not found.");

        await _repository.DeleteAsync(existing.Id);
        _logger.LogInformation("Student deleted with Id {StudentId}.", existing.Id);
    }
}
