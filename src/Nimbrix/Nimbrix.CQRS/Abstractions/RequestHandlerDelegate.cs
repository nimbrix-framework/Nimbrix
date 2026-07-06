namespace Nimbrix.CQRS.Abstractions;

public delegate ValueTask<TResponse> RequestHandlerDelegate<TResponse>();