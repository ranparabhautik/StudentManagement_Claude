using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Domain.Interfaces;
using StudentManagement.Infrastructure.Repositories;

namespace StudentManagement.Infrastructure.DependencyInjection;

/// <summary>
/// Extension methods to register infrastructure-layer services with the DI container.
/// </summary>
public static class InfrastructureServiceRegistration
{
    /// <summary>
    /// Registers the in-memory student repository as a singleton.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IStudentRepository, StudentRepository>();
        return services;
    }
}
