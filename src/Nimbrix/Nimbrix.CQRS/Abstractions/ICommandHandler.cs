namespace Nimbrix.CQRS.Abstractions;

/// <summary>
/// Represents a handler for processing commands in the CQRS (Command Query Responsibility Segregation) pattern.
/// Serves as a contract for executing commands of a specified type without returning a response.
/// </summary>
/// <typeparam name="TCommand">The type of command to be processed.</typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    ValueTask<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
}