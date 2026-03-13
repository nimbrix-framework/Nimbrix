using Microsoft.Extensions.Logging;
using Nimbrix.CQRS.Abstractions;
using Nimbrix.CQRS.Common;

namespace Nimbrix.CQRS.Pipeline;

/// <summary>
/// Implements a pipeline behavior that handles exceptions during the execution of a request in the CQRS pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request being processed.</typeparam>
/// <typeparam name="TResponse">The type of the response returned after processing the request.</typeparam>
/// <remarks>
/// This behavior logs any unhandled exceptions that occur during the execution of a request and gracefully handles them
/// by returning a failed <see cref="ServiceResult"/> or <see cref="ServiceResult{T}"/> if applicable. If the response type
/// does not match these patterns, the exception is re-thrown.
/// </remarks>
public sealed class ExceptionHandlingBehavior<TRequest, TResponse>(
    ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the processing of a request within a pipeline, enabling exception handling and logging.
    /// </summary>
    /// <param name="request">The request object to process.</param>
    /// <param name="next">The next delegate in the pipeline to invoke.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The response produced after processing the request.</returns>
    public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception while handling {Request}", typeof(TRequest).Name);

            if (typeof(TResponse) == typeof(ServiceResult))
            {
                object boxed = ServiceResult.Fail(["UnexpectedError"]);
                return (TResponse) boxed;
            }

            if (!typeof(TResponse).IsGenericType ||
                typeof(TResponse).GetGenericTypeDefinition() != typeof(ServiceResult<>))
            {
                throw;
            }

            var errorResult = Activator.CreateInstance(
                typeof(ServiceResult<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]),
                args: [false, null, "UnexpectedError"]);
                
            return (TResponse) errorResult!;

        }
    }
}