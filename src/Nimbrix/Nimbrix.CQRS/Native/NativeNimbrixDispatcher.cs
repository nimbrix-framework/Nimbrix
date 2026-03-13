using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nimbrix.CQRS.Abstractions;

namespace Nimbrix.CQRS.Native;

public sealed class NativeNimbrixDispatcher : INimbrixDispatcher
{
    private static readonly MethodInfo? _sendAsyncMethodInfo = typeof(NativeNimbrixDispatcher).GetMethod(nameof(SendAsync),
        [typeof(IRequest<>), typeof(CancellationToken)]);
    private readonly IServiceProvider _serviceProvider;

    public NativeNimbrixDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "Request cannot be null");
        }
        
        var requestType = request.GetType();
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = _serviceProvider.GetServices(behaviorType).ToArray();

        RequestHandlerDelegate<TResponse> handlerDelegate = () => InvokeHandlerViaReflectionAsync(_serviceProvider, request, cancellationToken);
        var pipeline = handlerDelegate;
        for (var i = behaviors.Length; i >= 0; i--)
        {
            var behavior = behaviors[i];
            var next = pipeline;
            
            pipeline = () => InvokeBehaviorAsync(behavior, request, next, cancellationToken);
        }
        
        return pipeline();
    }

    public ValueTask<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public ValueTask<TNotification> PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification => throw new NotImplementedException();

    private static ValueTask<TResponse> InvokeBehaviorAsync<TResponse>(object? behavior, object request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        MethodInfo? handleMethod = behavior?.GetType().GetMethod("HandleAsync");
        if (handleMethod is null)
        {
            throw new InvalidOperationException($"Pipeline behavior {behavior?.GetType().FullName} has no HandleAsync method.");
        }

        var result = handleMethod.Invoke(behavior, [request, next, cancellationToken]);
        if (result is ValueTask<TResponse> typedTask)
        {
            return typedTask;
        }

        throw new InvalidOperationException(
            $"Pipeline behavior {behavior?.GetType().FullName} returned an invalid result. Expected Task<TResponse>, got {result?.GetType().FullName ?? "null"}");
    }

    private static ValueTask<TResponse> InvokeHandlerViaReflectionAsync<TResponse>(IServiceProvider sp,
        IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        MethodInfo closed = InvokeHandlerMethod.MakeGenericMethod(request.GetType(), typeof(TResponse));
        var result = closed.Invoke(null, [sp, request, cancellationToken]);

        if (result is ValueTask<TResponse> typedTask)
        {
            return typedTask;
        }
        throw new InvalidOperationException("Handler invocation returned invalid task type.");
    }

    private static ValueTask<TResponse> InvokeHandlerAsync<TRequest, TResponse>(IServiceProvider sp,
        IRequest<TResponse> request, CancellationToken cancellationToken) where TRequest : ICommand<TResponse>, IQuery<TResponse>
    {
        switch (request)
        {
            case ICommand<TResponse> command:
            {
                var handler = sp.GetService<ICommandHandler<TRequest, TResponse>>();
                return handler is null ? throw new InvalidOperationException($"No command handler registered for {typeof(TRequest).FullName} -> {typeof(TResponse).FullName}") : handler.Handle((TRequest)command, cancellationToken);
            }
            case IQuery<TResponse> query:
            {
                var handler = sp.GetService<IQueryHandler<TRequest, TResponse>>();
                return handler?.HandleAsync((TRequest)query, cancellationToken) ?? throw new InvalidOperationException($"No query handler registered for {typeof(TRequest).FullName} -> {typeof(TResponse).FullName}");
            }
            default:
                throw new InvalidOperationException($"Request type {request.GetType().FullName} is not a command or query.");
        }
    }
    
    private static readonly MethodInfo InvokeHandlerMethod = typeof(NativeNimbrixDispatcher).GetMethod(nameof(InvokeHandlerAsync), BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("InvokeHandlerAsync method not found.");
}