namespace Nimbrix.CQRS.Configuration;

public enum NotificationFailureMode
{
    AggregateAndThrow,
    ThrowFirst,
    Ignore
}