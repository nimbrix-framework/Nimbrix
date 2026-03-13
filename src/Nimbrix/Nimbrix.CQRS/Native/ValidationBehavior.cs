using Microsoft.Extensions.DependencyInjection;
using Nimbrix.CQRS.Abstractions;

namespace Nimbrix.CQRS.Native;

public class ValidationBehavior<TRequest, TResponse>(IServiceProvider serviceProvider)
    : Pipeline.IPipelineBehavior<TRequest, TResponse>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task<TResponse> HandleAsync(TRequest request, Pipeline.RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validators = _serviceProvider.GetServices<IRequestValidator<TRequest>>().ToArray();
        if (validators.Length == 0)
        {
            return await next(cancellationToken);
        }
        
        var errors = await Task.WhenAll(validators.Select(v => v.ValidateAsync(request, cancellationToken)));
        var flat = errors.SelectMany(x => x).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        if (flat.Length > 0)
        {
            throw new InvalidOperationException("Validation failed: " + string.Join("; ", flat));
        }
        
        return await next(cancellationToken);
    }
}