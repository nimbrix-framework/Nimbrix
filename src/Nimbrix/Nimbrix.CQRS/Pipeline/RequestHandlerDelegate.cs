using Nimbrix.CQRS.Abstractions;

namespace Nimbrix.CQRS.Pipeline;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken cancellationToken = default);