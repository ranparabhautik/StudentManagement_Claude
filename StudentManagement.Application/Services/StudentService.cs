using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Application.Services;

/// <summary>
/// Implements student management business logic with validation and mapping.
/// </summary>
public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateStudentDto> _createValidator;
    private readonly IValidator<UpdateStudentDto> _updateValidator;
    private readonly ILogger<StudentService> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="StudentService"/>.
    /// </summary>
    /// <param name="repository">Student data repository.</param>
    /// <param name="mapper">AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="createValidator">Validator for <see cref="CreateStudentDto"/>.</param>
    /// <param name="updateValidator">Validator for <see cref="UpdateStudentDto"/>.</param>
    /// <param name="logger">Logger for this service.</param>
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

    /// <summary>Returns all students mapped to response DTOs.</summary>
    /// <returns>A collection of <see cref="StudentResponseDto"/>.</returns>
    public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
    {
        var students = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentResponseDto>>(students);
    }

    /// <summary>Returns a student by identifier, or <c>null</c> if not found.</summary>
    /// <param name="id">The student's unique identifier.</param>
    /// <returns>The matching <see cref="StudentResponseDto"/>, or <c>null</c>.</returns>
    public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);
        return student is null ? null : _mapper.Map<StudentResponseDto>(student);
    }

    /// <summary>Validates, creates, and persists a new student.</summary>
    /// <param name="dto">Student creation data.</param>
    /// <returns>The newly created student as a <see cref="StudentResponseDto"/>.</returns>
    /// <exception cref="ValidationException">Thrown when <paramref name="dto"/> fails validation.</exception>
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

    /// <summary>Validates and updates an existing student's data.</summary>
    /// <param name="dto">Updated student data, including the target identifier.</param>
    /// <returns>The updated student as a <see cref="StudentResponseDto"/>.</returns>
    /// <exception cref="ValidationException">Thrown when <paramref name="dto"/> fails validation.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no student with the given identifier exists.</exception>
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

    /// <summary>Deletes a student by identifier.</summary>
    /// <param name="id">The student's unique identifier.</param>
    /// <exception cref="KeyNotFoundException">Thrown when no student with the given identifier exists.</exception>
    public async Task DeleteStudentAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Student with Id {id} not found.");

        await _repository.DeleteAsync(existing.Id);
        _logger.LogInformation("Student deleted with Id {StudentId}.", existing.Id);
    }
}
