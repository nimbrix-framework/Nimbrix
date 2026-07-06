namespace Nimbrix.CQRS.Abstractions;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}