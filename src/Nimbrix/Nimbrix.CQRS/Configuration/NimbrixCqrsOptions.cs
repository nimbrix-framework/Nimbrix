namespace Nimbrix.CQRS.Configuration;

public class NimbrixCqrsOptions
{
    public NotificationPublishMode NotificationPublishMode { get; set; } = NotificationPublishMode.Parallel;
    public NotificationFailureMode NotificationFailureMode { get; set; } = NotificationFailureMode.AggregateAndThrow;
}