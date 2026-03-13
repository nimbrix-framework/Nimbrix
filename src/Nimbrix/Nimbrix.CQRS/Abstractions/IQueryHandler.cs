namespace Nimbrix.CQRS.Abstractions;

/// <summary>
/// Represents a generic interface for handling queries in the Command and Query
/// Responsibility Segregation (CQRS) pattern. It defines the contract for processing
/// a query and returning the corresponding response.
/// </summary>
/// <typeparam name="TQuery">
/// The type of the query to be handled, which must implement the <see cref="IQuery{TResponse}"/> interface.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of the response that is returned after processing the query.
/// </typeparam>
public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    public ValueTask<TResponse> HandleAsync(TQuery request, CancellationToken cancellationToken);
}