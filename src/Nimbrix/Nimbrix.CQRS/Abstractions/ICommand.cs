namespace Nimbrix.CQRS.Abstractions;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}