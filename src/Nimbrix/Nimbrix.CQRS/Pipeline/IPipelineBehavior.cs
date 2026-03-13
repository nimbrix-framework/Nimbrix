using Nimbrix.CQRS.Abstractions;

namespace Nimbrix.CQRS.Pipeline;

public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}