using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Nimbrix.AspDotNet.Requests;
using Nimbrix.AspDotNet.Results;
using Nimbrix.CQRS.Abstractions;

namespace Nimbrix.AspDotNet.Controller;

public partial class CqrsBaseController<TResource, TUser>(IStringLocalizer<TResource> localizer, INimbrixDispatcher nimbrixDispatcher, ILogger<CqrsBaseController<TResource, TUser>> logger) : ControllerBase
{
    // private ISender Mediator => field ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private IStringLocalizer<TResource> Localizer
    {
        get => field ??= HttpContext.RequestServices.GetRequiredService<IStringLocalizer<TResource>>();
    } = localizer;

    private ILogger<CqrsBaseController<TResource, TUser>> Logger
    {
        get => field ??= HttpContext.RequestServices
            .GetRequiredService<ILogger<CqrsBaseController<TResource, TUser>>>();
    } = logger;

    protected bool IsAuthenticated => HttpContext.User.Identity?.IsAuthenticated ?? false;
    protected string? CurrentUserEmail => HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    protected IActionResult OkResponse<T>(T data, string titleKey = "SuccesDefaultTitle")
    {
        return Ok(ApiResponse<T>.CreateSuccess(data, Localizer[titleKey]));
    }

    protected IActionResult CreatedResponse<T>(T data, string createdAt, object? routeValues = null,
        string titleKey = "CreatedDefaultTitle", string messageKey = "CreatedDefaultMessage")
    {
        return CreatedAtRoute(createdAt, routeValues, ApiResponse<T>.CreateSuccess(data, Localizer[titleKey], StatusCodes.Status201Created));
    }

    protected IActionResult BadRequestResponse(string errorKey, object? routeValues = null)
    {
        return StatusCode(StatusCodes.Status400BadRequest, ApiResponse.CreateError(Localizer[errorKey], StatusCodes.Status400BadRequest));
    }

    protected IActionResult NotFoundResponse(string errorKey)
    {
        return StatusCode(StatusCodes.Status404NotFound, ApiResponse.CreateError(Localizer[errorKey], StatusCodes.Status404NotFound));
    }

    protected IActionResult UnauthorizedResponse(string errorKey)
    {
        return StatusCode(StatusCodes.Status401Unauthorized, ApiResponse.CreateError(Localizer[errorKey], StatusCodes.Status401Unauthorized));
    }

    protected IActionResult ForbiddenResponse(string errorKey)
    {
        return StatusCode(StatusCodes.Status403Forbidden, ApiResponse.CreateError(Localizer[errorKey], StatusCodes.Status403Forbidden));
    }

    protected IActionResult InternalServerErrorResponse(string errorKey)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.CreateError(Localizer[errorKey], StatusCodes.Status500InternalServerError));
    }

    protected async Task<IActionResult> HandleRequestAsync(CqrsRequest request, CancellationTokenSource cancellationTokenSource)
    {
        var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        try
        {
            var searchType = request.Type.Replace(".", "+");
            var type = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(p => p.FullName?.EndsWith(searchType) == true);

            if (type is null)
            {
                LogCouldNotFindTypeType(Logger, request.Type);
                return BadRequest(ApiResponse.CreateError($"Unknown Cqrs type: {request.Type}", statusCode: StatusCodes.Status400BadRequest));
            }

            var allowAnonymous = type.GetCustomAttribute<AllowAnonymousAttribute>() != null;
            var authorizeAttributes = type.GetCustomAttributes<AuthorizeAttribute>().ToList();

            var requireAuthenticatedUser = !allowAnonymous && authorizeAttributes.Count > 0;
            
            var user = HttpContext.User;
            var isAuthenticated = user.Identity?.IsAuthenticated ?? false;

            if (requireAuthenticatedUser && !isAuthenticated)
            {
                LogUserIsNotAuthenticatedForType(Logger, request.Type);
                return Unauthorized();
            }
            
            var requiredRoles  = authorizeAttributes.Where(x => !string.IsNullOrWhiteSpace(x.Roles))
                .SelectMany(x => x.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (requireAuthenticatedUser && requiredRoles.Count > 0)
            {
                var hasRequiredRole = user.IsInRole(requiredRoles.First());
                if (!hasRequiredRole)
                {
                    LogUserDoesNotHaveRequiredRoleRoleForType(Logger, requiredRoles.First(), request.Type);
                    return Forbid();
                }
            }

            if (request.Payload.Deserialize(type, serializerOptions) is not IRequest<object?> payload)
            {
                return StatusCode(StatusCodes.Status400BadRequest,ApiResponse.CreateError("BadRequest", "The payload must not be empty.", statusCode: StatusCodes.Status400BadRequest));
            }
            
            var response = await nimbrixDispatcher.SendAsync(payload, cancellationTokenSource.Token);
            return OkResponse(response);
        }
        catch (Exception e)
        {
            LogErrorHandlingCqrsRequestForType(Logger, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse.CreateError("UnexpectedError", statusCode: StatusCodes.Status500InternalServerError));
        }
    }

    private async Task<T> SendAsync<T>(IRequest<T> payload, CancellationToken token)
    {
        throw new NotImplementedException("Not implemented yet.");
    }

    [LoggerMessage(LogLevel.Warning, "Could not find type {type}")]
    static partial void LogCouldNotFindTypeType(ILogger<CqrsBaseController<TResource, TUser>> logger, string type);

    [LoggerMessage(LogLevel.Warning, "User is not authenticated for {type}")]
    static partial void LogUserIsNotAuthenticatedForType(ILogger<CqrsBaseController<TResource, TUser>> logger, string type);

    [LoggerMessage(LogLevel.Information, "User does not have required role {role} for {type}")]
    static partial void LogUserDoesNotHaveRequiredRoleRoleForType(ILogger<CqrsBaseController<TResource, TUser>> logger, string role, string type);

    [LoggerMessage(LogLevel.Error, "{message}")]
    static partial void LogErrorHandlingCqrsRequestForType(ILogger<CqrsBaseController<TResource, TUser>> logger, string message);
}