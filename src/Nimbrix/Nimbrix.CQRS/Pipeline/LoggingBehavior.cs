using Microsoft.Extensions.Logging;
using Nimbrix.CQRS.Abstractions;

namespace Nimbrix.CQRS.Pipeline;

/// <summary>
/// Represents a pipeline behavior that logs the processing of requests and their responses.
/// </summary>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by handling the request.</typeparam>
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles a request within the pipeline and logs the processing of the request and its response.
    /// </summary>
    /// <param name="request">The request being processed.</param>
    /// <param name="next">The delegate representing the next step in the pipeline.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>The response produced by processing the request.</returns>
    public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogDebug("Handling request {RequestName}", requestName);
        var response = await next(cancellationToken);
        _logger.LogDebug("Handled request {RequestName}", requestName);
        return response;
    }
}