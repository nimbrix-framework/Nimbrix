using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nimbrix.AspDotNet.Results;

/// <summary>
/// Represents a standardized response for API operations that implements the IActionResult interface.
/// </summary>
public class ApiResponse : IActionResult
{
    /// <summary>
    /// Gets or sets the title of the API response, which typically represents a summary or
    /// classification of the outcome of the operation (e.g., "Success", "Error").
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets an array of error messages, typically used to convey detailed information
    /// about issues or validation failures that occurred during the execution of an API operation.
    /// </summary>
    public string[]? Errors { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code associated with the API response.
    /// This represents the outcome of the operation, such as 200 for success or 400 for a bad request.
    /// </summary>
    protected int StatusCode { get; init; } = StatusCodes.Status200OK;

    /// <summary>
    /// Creates a successful API response with the specified title and message.
    /// </summary>
    /// <param name="title">The title of the success response. If null, defaults to "Success".</param>
    /// <returns>An <see cref="ApiResponse"/> instance representing a successful response.</returns>
    public static ApiResponse CreateSuccess(string? title)
    {
        return new ApiResponse { Title = title ?? "Success" };
    }

    /// <summary>
    /// Creates a successful API response with the specified title and status code.
    /// </summary>
    /// <param name="title">The title of the success response. If null, no title will be included in the response.</param>
    /// <param name="statusCode">The HTTP status code to associate with the response.</param>
    /// <returns>An <see cref="ApiResponse"/> instance representing a successful response with the specified status code.</returns>
    public static ApiResponse CreateSuccess(string? title, int statusCode)
    {
        return new ApiResponse
        {
            Title = title,
            StatusCode = statusCode,
        };
    }

    /// <summary>
    /// Creates an error API response with the specified title, message, and status code.
    /// </summary>
    /// <param name="title">The title of the error response. If null, defaults to "Error".</param>
    /// <param name="message">The message describing the error. If null, defaults to an empty string.</param>
    /// <param name="statusCode">The HTTP status code representing the error.</param>
    /// <returns>An <see cref="ApiResponse"/> instance representing an error response.</returns>
    public static ApiResponse CreateError(string? title, string? message, int statusCode)
    {
        return new ApiResponse
        {
            Title = title ?? "Error",
            Errors = [message ?? string.Empty],
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates an error API response with the specified error message, status code, and optional validation errors.
    /// </summary>
    /// <param name="error">The primary error message for the response.</param>
    /// <param name="statusCode">The HTTP status code representing the error.</param>
    /// <param name="validationErrors">
    /// A dictionary containing validation error details, where the key represents the field name and the value represents the associated error message. If null, no validation errors are included.
    /// </param>
    /// <returns>An <see cref="ApiResponse"/> instance representing an error response.</returns>
    public static ApiResponse CreateError(string error, int statusCode,
        Dictionary<string, string>? validationErrors = null)
    {
        var errorMessages = validationErrors?.Select(x => $"{x.Key}: {x.Value}").ToArray();

        return new ApiResponse
        {
            Title = error,
            Errors = errorMessages,
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Executes the result operation of the current API response within the given action context.
    /// </summary>
    /// <param name="context">The context in which the result is executed. Provides access to HTTP-related information about a request.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation of executing the result.</returns>
    public virtual Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(this)
        {
            StatusCode = StatusCode
        };
        return objectResult.ExecuteResultAsync(context);
    }
}

/// <summary>
/// Represents a standardized response for API operations that optionally includes data of a specific type.
/// </summary>
/// <typeparam name="T">The type of data included in the response.</typeparam>
public class ApiResponse<T> : ApiResponse
{
    /// <summary>
    /// Gets or sets the data associated with the API response. This property typically holds the payload
    /// of the response when the operation is successful, allowing strongly typed access to the returned data.
    /// </summary>
    /// <typeparam name="T">The type of the data included in the response.</typeparam>
    public T? Data { get; set; }

    /// <summary>
    /// Executes the result operation of the standardized API response asynchronously in the specified context.
    /// </summary>
    /// <param name="context">The context in which the result is executed, containing information about the current HTTP request and response.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous execution of the result operation.</returns>
    public override Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(this)
        {
            StatusCode = StatusCode
        };
        return objectResult.ExecuteResultAsync(context);
    }

    /// <summary>
    /// Creates a successful API response with the specified data, title, message, and status code.
    /// </summary>
    /// <param name="data">The data to include in the success response.</param>
    /// <param name="error">The title of the response. Defaults to "Success" if not specified.</param>
    /// <param name="message">The message of the success response. If null, defaults to an empty string.</param>
    /// <param name="statusCode">The HTTP status code for the response. Defaults to 200 (OK).</param>
    /// <returns>An instance of <see cref="ApiResponse{T}"/> representing a successful response.</returns>
    public static ApiResponse<T> CreateSuccess(T data, string? error = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Title = error,
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates an error API response with the specified error message, details, and HTTP status code.
    /// </summary>
    /// <param name="error">The title of the error response.</param>
    /// <param name="message">A detailed message explaining the error.</param>
    /// <param name="statusCode">The HTTP status code for the error response.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> instance representing an error response.</returns>
    public new static ApiResponse<T> CreateError(string error, string message, int statusCode)
    {
        return new ApiResponse<T>
        {
            Title = error,
            Errors = [message],
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates an error API response with the specified error message, optional validation errors, and an HTTP status code.
    /// </summary>
    /// <param name="error">A brief description of the error.</param>
    /// <param name="validationErrors">A dictionary of field-specific validation errors, where the key is the field name, and the value is the error message. Set to null if not applicable.</param>
    /// <param name="statusCode">The HTTP status code for the error response. Defaults to 400 (Bad Request).</param>
    /// <returns>An <see cref="ApiResponse{T}"/> instance representing an error response.</returns>
    public static ApiResponse<T> CreateError(string error, Dictionary<string, string>? validationErrors = null,
        int statusCode = StatusCodes.Status400BadRequest)
    {
        return new ApiResponse<T>
        {
            Title = error,
            Errors = validationErrors?.Select(x => $"{x.Key}: {x.Value}").ToArray(),
            StatusCode = statusCode
        };
    }
}