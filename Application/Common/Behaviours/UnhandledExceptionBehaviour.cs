using Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Common.Behaviours;
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found in {RequestName}: {Message}",
                typeof(TRequest).Name, ex.Message);

            throw new HttpResponseException(HttpStatusCode.NotFound, ex.Message);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Business exception occurred in {RequestName}: {ErrorCode} - {Message}",
                typeof(TRequest).Name, ex.StatusCode, ex.Message);

            throw new HttpResponseException(ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {RequestName}: {@Request}",
                typeof(TRequest).Name, request);

            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }
}
