namespace Nimbrix.CQRS.Common;

public class ServiceError(string code, string message)
{
    public string Code { get; } = code;
    public string Message { get; } = message;
}