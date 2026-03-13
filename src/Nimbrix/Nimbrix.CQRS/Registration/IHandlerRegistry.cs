namespace Nimbrix.CQRS.Registration;

public interface IHandlerRegistry
{
    void RegisterCommand<TCommand, THandler, TResponse>()
        where TCommand : Abstractions.ICommand<TResponse>
        where THandler : Abstractions.ICommandHandler<TCommand, TResponse>;
    
    void RegisterQuery<TQuery, THandler, TResponse>()
        where TQuery : Abstractions.IQuery<TResponse>
        where THandler : Abstractions.IQueryHandler<TQuery, TResponse>;
    
    Type GetCommandHandlerType(Type commandType);
    Type GetQueryHandlerType(Type queryType);
}