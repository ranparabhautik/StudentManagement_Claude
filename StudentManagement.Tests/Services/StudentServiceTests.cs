using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Mapping;
using StudentManagement.Application.Services;
using StudentManagement.Application.Validators;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;
using Xunit;

namespace StudentManagement.Tests.Services;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateStudentDto> _createValidator;
    private readonly IValidator<UpdateStudentDto> _updateValidator;
    private readonly StudentService _sut;

    public StudentServiceTests()
    {
        _mockRepo = new Mock<IStudentRepository>();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<StudentMappingProfile>());
        _mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();

        _createValidator = new CreateStudentDtoValidator();
        _updateValidator = new UpdateStudentDtoValidator();

        var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<StudentService>>();
        _sut = new StudentService(_mockRepo.Object, _mapper, _createValidator, _updateValidator, mockLogger.Object);
    }

    // ── Happy Path ────────────────────────────────────────────────────────

    [Fact]
    public async Task AddStudentAsync_ValidDto_ReturnsStudentResponseDtoWithAssignedId()
    {
        // Arrange
        var dto = new CreateStudentDto
        {
            Name = "Alice Smith",
            Email = "alice@example.com",
            EnrollmentDate = new DateTime(2024, 9, 1),
            Grade = "A"
        };

        _mockRepo
            .Setup(r => r.AddAsync(It.IsAny<Student>()))
            .Callback<Student>(s => s.Id = 1)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.AddStudentAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Email, result.Email);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task GetStudentByIdAsync_ExistingId_ReturnsCorrectStudent()
    {
        // Arrange
        var student = new Student { Id = 5, Name = "Bob", Email = "bob@example.com", EnrollmentDate = DateTime.Today, Grade = "B" };
        _mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(student);

        // Act
        var result = await _sut.GetStudentByIdAsync(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result!.Id);
        Assert.Equal("Bob", result.Name);
    }

    [Fact]
    public async Task UpdateStudentAsync_ValidDto_ReturnsUpdatedStudent()
    {
        // Arrange
        var existing = new Student { Id = 3, Name = "Old Name", Email = "old@example.com", EnrollmentDate = new DateTime(2023, 1, 1), Grade = "C" };
        var dto = new UpdateStudentDto { Id = 3, Name = "New Name", Email = "new@example.com", EnrollmentDate = new DateTime(2023, 1, 1), Grade = "A" };

        _mockRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.UpdateStudentAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Id);
        Assert.Equal("New Name", result.Name);
        Assert.Equal("A", result.Grade);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Student>()), Times.Once);
    }

    // ── Edge Case ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllStudentsAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        // Act
        var result = await _sut.GetAllStudentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    // ── Negative Cases ────────────────────────────────────────────────────

    [Fact]
    public async Task GetStudentByIdAsync_NonExistentId_ReturnsNull()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Student?)null);

        // Act
        var result = await _sut.GetStudentByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteStudentAsync_NonExistentId_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _sut.DeleteStudentAsync(999));
    }
}
