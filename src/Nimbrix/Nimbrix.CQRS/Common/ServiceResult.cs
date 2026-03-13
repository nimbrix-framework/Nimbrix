using Microsoft.AspNetCore.Http;

namespace Nimbrix.CQRS.Common;

/// <summary>
/// Encapsulates the result of a service operation, indicating success or failure,
/// and optionally providing error details or additional data.
/// </summary>
public readonly record struct ServiceResult
{
    /// <summary>
    /// Gets a value indicating whether the operation represented by the <see cref="ServiceResult"/>
    /// was successful. A value of <c>true</c> indicates success, while <c>false</c> indicates failure.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets an array of error messages associated with the operation represented by the <see cref="ServiceResult"/>.
    /// When the operation fails, this property contains the corresponding error descriptions.
    /// If the operation is successful, this property is <c>null</c>.
    /// </summary>
    public string[]? Errors { get; }

    /// <summary>
    /// Gets the HTTP status code that represents the outcome of the service operation.
    /// This code provides additional context about the result, such as whether it
    /// succeeded or failed, and the nature of any error if applicable.
    /// </summary>
    public int StatusCode { get; }

    private ServiceResult(bool isSuccess, string[]? errors, int statusCode = StatusCodes.Status200OK)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Represents a successful operation result without any associated value or error.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="ServiceResult"/> indicating a successful operation.
    /// </returns>
    public static ServiceResult Ok() => new(true, null);

    /// <summary>
    /// Represents a failed operation result with a set of associated error messages.
    /// </summary>
    /// <param name="errors">An array of error messages describing the failure.</param>
    /// <returns>
    /// A new instance of <see cref="ServiceResult"/> indicating a failed operation with the specified errors.
    /// </returns>
    public static ServiceResult Fail(string[] errors) => new(false, errors, StatusCodes.Status400BadRequest);

    /// <summary>
    /// Represents a successful operation result without any associated value or error.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="ServiceResult"/> indicating a successful operation.
    /// </returns>
    public static ServiceResult<T> Ok<T>(T value) => new(true, value, null);

    /// <summary>
    /// Represents a failed operation result with a set of associated error messages.
    /// </summary>
    /// <typeparam name="T">The type of the value, ignored in the case of a failed operation.</typeparam>
    /// <param name="error">An array of error messages describing the failure.</param>
    /// <returns>
    /// A new instance of <see cref="ServiceResult{T}"/> indicating a failed operation with the specified errors.
    /// </returns>
    public static ServiceResult<T> Fail<T>(string[] error) => new(false, default, error, StatusCodes.Status400BadRequest);
}

/// <summary>
/// Represents the result of a service operation, indicating success or failure,
/// and optionally including error messages.
/// </summary>
public readonly record struct ServiceResult<T>
{
    /// <summary>
    /// Indicates whether the operation represented by the current instance
    /// completed successfully. Returns <c>true</c> for a successful operation,
    /// and <c>false</c> for a failed one.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the value associated with the result of the operation, if the operation was successful.
    /// A non-<c>null</c> value is returned for successful results, while a failed result will typically have a <c>null</c> value.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Gets an array of error messages associated with a failed operation. If the operation
    /// was successful, the value will be <c>null</c>.
    /// </summary>
    public string[]? Errors { get; }

    /// <summary>
    /// Gets the HTTP status code associated with the result of the service operation.
    /// This value indicates the outcome of the operation as a standard HTTP response status code,
    /// such as 200 for success or 400 for a client error.
    /// </summary>
    public int StatusCode { get; }

    internal ServiceResult(bool isSuccess, T? value, string[]? errors, int statusCode = StatusCodes.Status200OK)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
        StatusCode = statusCode;
    }
}