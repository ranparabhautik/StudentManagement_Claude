using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using StudentManagement.Application.DependencyInjection;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Infrastructure.DependencyInjection;

// ── Serilog setup ──────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "Logs/app-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

// ── DI container ───────────────────────────────────────────────────────────
var services = new ServiceCollection();
services
    .AddApplicationServices()
    .AddInfrastructureServices()
    .AddLogging(b => { b.ClearProviders(); b.AddSerilog(dispose: true); });

await using var provider = services.BuildServiceProvider();
var studentService = provider.GetRequiredService<IStudentService>();
var logger = provider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Student Management System started.");

// ── Menu loop ──────────────────────────────────────────────────────────────
while (true)
{
    Console.WriteLine();
    Console.WriteLine("╔══════════════════════════════════╗");
    Console.WriteLine("║    Student Management System     ║");
    Console.WriteLine("╠══════════════════════════════════╣");
    Console.WriteLine("║  1. Add Student                  ║");
    Console.WriteLine("║  2. View All Students            ║");
    Console.WriteLine("║  3. Find Student by ID           ║");
    Console.WriteLine("║  4. Update Student               ║");
    Console.WriteLine("║  5. Delete Student               ║");
    Console.WriteLine("║  6. Exit                         ║");
    Console.WriteLine("╚══════════════════════════════════╝");
    Console.Write("Select option: ");

    var choice = Console.ReadLine()?.Trim();
    Console.WriteLine();

    try
    {
        switch (choice)
        {
            case "1": await AddStudentAsync(); break;
            case "2": await ViewAllStudentsAsync(); break;
            case "3": await FindByIdAsync(); break;
            case "4": await UpdateStudentAsync(); break;
            case "5": await DeleteStudentAsync(); break;
            case "6":
                logger.LogInformation("Application exited by user.");
                Console.WriteLine("Goodbye!");
                return;
            default:
                Console.WriteLine("Invalid option. Please enter 1–6.");
                break;
        }
    }
    catch (ValidationException ex)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Validation errors:");
        foreach (var err in ex.Errors)
            Console.WriteLine($"  • {err.PropertyName}: {err.ErrorMessage}");
        Console.ResetColor();
        logger.LogWarning("Validation failed: {Errors}", string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)));
    }
    catch (KeyNotFoundException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Not found: {ex.Message}");
        Console.ResetColor();
        logger.LogWarning("Not found: {Message}", ex.Message);
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Unexpected error: {ex.Message}");
        Console.ResetColor();
        logger.LogError(ex, "Unexpected error.");
    }
}

// ── Handlers ───────────────────────────────────────────────────────────────

async Task AddStudentAsync()
{
    Console.WriteLine("── Add Student ──────────────────");
    var dto = new CreateStudentDto
    {
        Name = Prompt("Name"),
        Email = Prompt("Email"),
        EnrollmentDate = PromptDate("Enrollment Date (yyyy-MM-dd)"),
        Grade = Prompt("Grade (e.g. A, B+, C)")
    };

    var result = await studentService.AddStudentAsync(dto);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Student added with ID: {result.Id}");
    Console.ResetColor();
    logger.LogInformation("Added student Id={Id} Name={Name}", result.Id, result.Name);
}

async Task ViewAllStudentsAsync()
{
    var students = (await studentService.GetAllStudentsAsync()).ToList();

    if (students.Count == 0)
    {
        Console.WriteLine("No students found.");
        return;
    }

    Console.WriteLine($"{"ID",-5} {"Name",-25} {"Email",-30} {"Enrollment",-12} {"Grade",-6}");
    Console.WriteLine(new string('─', 80));
    foreach (var s in students)
        Console.WriteLine($"{s.Id,-5} {s.Name,-25} {s.Email,-30} {s.EnrollmentDate:yyyy-MM-dd,-12} {s.Grade,-6}");
}

async Task FindByIdAsync()
{
    int id = PromptInt("Student ID");
    var student = await studentService.GetStudentByIdAsync(id);

    if (student is null)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"No student found with ID {id}.");
        Console.ResetColor();
        return;
    }

    PrintStudent(student);
}

async Task UpdateStudentAsync()
{
    Console.WriteLine("── Update Student ───────────────");
    var dto = new UpdateStudentDto
    {
        Id = PromptInt("Student ID to update"),
        Name = Prompt("New Name"),
        Email = Prompt("New Email"),
        EnrollmentDate = PromptDate("New Enrollment Date (yyyy-MM-dd)"),
        Grade = Prompt("New Grade (e.g. A, B+, C)")
    };

    var result = await studentService.UpdateStudentAsync(dto);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Student ID {result.Id} updated successfully.");
    Console.ResetColor();
    logger.LogInformation("Updated student Id={Id}", result.Id);
}

async Task DeleteStudentAsync()
{
    int id = PromptInt("Student ID to delete");
    Console.Write($"Are you sure you want to delete student {id}? (y/n): ");
    if (!Console.ReadLine()?.Trim().Equals("y", StringComparison.OrdinalIgnoreCase) ?? true)
    {
        Console.WriteLine("Cancelled.");
        return;
    }

    await studentService.DeleteStudentAsync(id);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Student ID {id} deleted.");
    Console.ResetColor();
    logger.LogInformation("Deleted student Id={Id}", id);
}

// ── Helpers ────────────────────────────────────────────────────────────────

static string Prompt(string label)
{
    Console.Write($"{label}: ");
    return Console.ReadLine()?.Trim() ?? string.Empty;
}

static int PromptInt(string label)
{
    while (true)
    {
        Console.Write($"{label}: ");
        if (int.TryParse(Console.ReadLine(), out var val)) return val;
        Console.WriteLine("  Please enter a valid number.");
    }
}

static DateTime PromptDate(string label)
{
    while (true)
    {
        Console.Write($"{label}: ");
        if (DateTime.TryParse(Console.ReadLine(), out var d)) return d;
        Console.WriteLine("  Please enter a valid date (e.g. 2024-09-01).");
    }
}

static void PrintStudent(StudentResponseDto s)
{
    Console.WriteLine($"  ID            : {s.Id}");
    Console.WriteLine($"  Name          : {s.Name}");
    Console.WriteLine($"  Email         : {s.Email}");
    Console.WriteLine($"  Enrollment    : {s.EnrollmentDate:yyyy-MM-dd}");
    Console.WriteLine($"  Grade         : {s.Grade}");
}
