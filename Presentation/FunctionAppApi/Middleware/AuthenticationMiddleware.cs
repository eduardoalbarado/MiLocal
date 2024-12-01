using Application.Common.Models.Responses;
using Application.Features.Users.Commands.AddUser;
using Application.Interfaces;
using FunctionAppApi.Extensions;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FunctionAppApi.Middleware;

public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly IUserContextService _userContextService;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private readonly bool _isDebug;

    public AuthenticationMiddleware(IUserContextService userContextService, IMediator mediator, ILogger<AuthenticationMiddleware> logger)
    {
        _userContextService = userContextService;
        _mediator = mediator;
        _logger = logger;
        _isDebug = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot") != null;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        context.TryExtractHttpRequestData(out var req);

        // Permitir acceso a /swagger sin autenticación
        if (req != null && req.Url.PathAndQuery.Contains("/swagger"))
        {
            await next(context);
            return;
        }

        // Verificar la existencia del header Authorization
        if (!req.Headers.TryGetValues("Authorization", out var authorization) || !authorization.Any())
        {
            if (!_isDebug)
            {
                await SendUnauthorizedResponse(context, req, "Invalid token or user context.");
                return;
            }
        }

        var token = authorization?.First().Replace("Bearer ", "") ?? string.Empty;
        var userContext = _isDebug ? GetUserContextFromDebugToken(token) : GetUserContextFromToken(token);

        if (userContext == null)
        {
            await SendUnauthorizedResponse(context, req, "Invalid token or user context.");
            return;
        }

        // Guardar el contexto de usuario
        _userContextService.SetUserContext(userContext);

        // Verificar si el usuario existe en la base de datos
        bool userExists;
        try
        {
            userExists = await _userContextService.UserExistsAsync(userContext.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connection failed while checking if user exists.");
            await SendErrorResponse(context, req, "Database Error", "Unable to connect to the database. Please try again later.");
            return;
        }

        if (!userExists)
        {
            var addUserCommand = new AddUserCommand
            {
                UserId = userContext.UserId,
                UserName = userContext.UserName,
                Email = userContext.Email
            };

            try
            {
                await _mediator.Send(addUserCommand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new user.");
                await SendErrorResponse(context, req, "User creation failed", ex.Message);
                return;
            }
        }

        await next(context);
    }

    private UserContext GetUserContextFromDebugToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(token))
            {
                var claimsPrincipal = tokenHandler.ReadJwtToken(token);
                string userId = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                string userName = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
                string email = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

                return string.IsNullOrEmpty(userId) ? null : new UserContext(userId, userName, email);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing debug token.");
        }
        return null;
    }

    private UserContext GetUserContextFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = "tu_issuer",
                ValidAudience = "tu_audience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret_key_mockup")),
                ClockSkew = TimeSpan.Zero
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);
            string userId = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
            string userName = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
            string email = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            return string.IsNullOrEmpty(userId) ? null : new UserContext(userId, userName, email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token.");
        }
        return null;
    }

    private async Task SendUnauthorizedResponse(FunctionContext context, HttpRequestData req, string message)
    {
        var response = req.CreateResponse();
        await response.WriteAsJsonAsync(new UnauthorizedResponse { Message = message }, HttpStatusCode.Unauthorized);
        context.SendResponseAsync(response);
    }

    private async Task SendErrorResponse(FunctionContext context, HttpRequestData req, string title, string detail)
    {
        var response = req.CreateResponse();
        await response.WriteAsJsonAsync(new ErrorResponse(title, detail), HttpStatusCode.InternalServerError);
        context.SendResponseAsync(response);
    }
}
