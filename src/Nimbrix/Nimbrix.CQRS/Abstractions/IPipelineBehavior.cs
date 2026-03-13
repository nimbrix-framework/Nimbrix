namespace Nimbrix.CQRS.Abstractions;

public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    ValueTask<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}