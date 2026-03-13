using Nimbrix.CQRS.Abstractions;
using Nimbrix.CQRS.Exceptions;

namespace Nimbrix.CQRS.Registration;

public class HandlerRegistration : IHandlerRegistry
{
    private readonly Dictionary<Type, Type> _commandHandlers = new();
    private readonly Dictionary<Type, Type> _queryHandlers = new();
    
    public void RegisterCommand<TCommand, THandler, TResponse>() where TCommand : ICommand<TResponse> where THandler : ICommandHandler<TCommand, TResponse>
    {
        var commandType = typeof(TCommand);
        var handlerType = typeof(THandler);

        if (!_commandHandlers.TryAdd(commandType, handlerType))
        {
            throw new MultipleHandlerException(commandType);
        }
    }

    public void RegisterQuery<TQuery, THandler, TResponse>() where TQuery : IQuery<TResponse> where THandler : IQueryHandler<TQuery, TResponse>
    {
        var queryType = typeof(TQuery);
        var handlerType = typeof(THandler);

        if (!_queryHandlers.TryAdd(queryType, handlerType))
        {
            throw new MultipleHandlerException(queryType);
        }
    }

    public Type GetCommandHandlerType(Type commandType) => !_commandHandlers.TryGetValue(commandType, out var commandHandlerType) ? throw new MissingHandlerException(commandType) : commandHandlerType;

    public Type GetQueryHandlerType(Type queryType) => !_queryHandlers.TryGetValue(queryType, out var queryHandlerType) ? throw new MissingHandlerException(queryType) : queryHandlerType;
}