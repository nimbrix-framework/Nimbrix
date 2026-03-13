namespace Nimbrix.CQRS.Exceptions;

public class MissingHandlerException(Type requestType)
    : NimbrixCqrsException($"No handler registered for requested type {requestType.Name}");