namespace Nimbrix.CQRS.Abstractions;

public interface IRequestValidator<in TRequest>
{
    Task<IReadOnlyList<string>> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}