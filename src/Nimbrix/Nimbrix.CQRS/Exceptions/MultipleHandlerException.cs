namespace Nimbrix.CQRS.Exceptions;

public class MultipleHandlerException(Type requestType)
    : NimbrixCqrsException($"Multiple handler registered for request type {requestType.Name}");