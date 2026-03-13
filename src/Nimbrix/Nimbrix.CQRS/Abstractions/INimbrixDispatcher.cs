namespace Nimbrix.CQRS.Abstractions;

public interface INimbrixDispatcher
{
    ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    ValueTask<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    ValueTask<TNotification> PublishAsync<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default) where TNotification : INotification;
}