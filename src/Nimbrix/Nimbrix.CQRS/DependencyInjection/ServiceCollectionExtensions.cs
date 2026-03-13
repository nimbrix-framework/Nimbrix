using Microsoft.Extensions.DependencyInjection;
using Nimbrix.CQRS.Abstractions;
using Nimbrix.CQRS.Native;
using Nimbrix.CQRS.Pipeline;

namespace Nimbrix.CQRS.DependencyInjection;

/// <summary>
/// Provides extension methods for configuring Nimbrix CQRS-related services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Nimbrix CQRS services to the specified <see cref="IServiceCollection"/>.
    /// This includes registering the mediator, as well as pipeline behaviors
    /// such as logging and exception handling.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddNimbrixCqrs(this IServiceCollection services)
    {
        services.AddScoped(typeof(Pipeline.IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(Pipeline.IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
        services.AddScoped<INimbrixDispatcher, NativeNimbrixDispatcher>();
        services.AddScoped(typeof(Abstractions.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}