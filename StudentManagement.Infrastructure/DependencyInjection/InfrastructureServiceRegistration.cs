using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Domain.Interfaces;
using StudentManagement.Infrastructure.Repositories;

namespace StudentManagement.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IStudentRepository, StudentRepository>();
        return services;
    }
}
