using Microsoft.Extensions.DependencyInjection;

namespace Nimbrix.AspDotNet;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> to register services specific to the Nimbrix ASP.NET framework.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNimbrix(this IServiceCollection services) => services;
}