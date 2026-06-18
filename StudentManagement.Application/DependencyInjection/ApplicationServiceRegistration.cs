using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Mapping;
using StudentManagement.Application.Services;

namespace StudentManagement.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<StudentMappingProfile>());
        services.AddValidatorsFromAssembly(typeof(StudentMappingProfile).Assembly);
        services.AddScoped<IStudentService, StudentService>();
        return services;
    }
}
