namespace Nimbrix.CQRS.Common;

public class ServiceValidationError(string field, string code, string message) : ServiceError(code, message)
{
    public string Field { get; } = field;
}