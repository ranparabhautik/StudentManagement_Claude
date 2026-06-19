using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Mapping;
using StudentManagement.Application.Services;

namespace StudentManagement.Application.DependencyInjection;

/// <summary>
/// Extension methods to register application-layer services with the DI container.
/// </summary>
public static class ApplicationServiceRegistration
{
    /// <summary>
    /// Registers AutoMapper profiles, FluentValidation validators, and application services.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<StudentMappingProfile>());
        services.AddValidatorsFromAssembly(typeof(StudentMappingProfile).Assembly);
        services.AddScoped<IStudentService, StudentService>();
        return services;
    }
}
