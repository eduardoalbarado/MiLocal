using Application.Common.Models.Responses;
using Application.Interfaces;
using FunctionAppApi.Extensions;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace FunctionAppApi.Middleware
{
    public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
    {
        private ILogger _logger;
        private readonly IUserContextService _userContextService;
        private readonly bool _isDebug;

        public AuthenticationMiddleware(IUserContextService userContextService)
        {
            _userContextService = userContextService;
            _isDebug = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot") != null;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            _logger = context.GetLogger<AuthenticationMiddleware>();


            context.TryExtractHttpRequestData(out var req);

            if (context.TryExtractHttpRequestData(out var requestSwagger))
            {
                if (requestSwagger.Url.PathAndQuery.Contains("/swagger"))
                {
                    await next(context);
                    return;
                }
            }

            // Check if the request contains the authorization header
            if (!req.Headers.TryGetValues("Authorization", out var authorization) || !authorization.Any())
            {
                if (!_isDebug)
                {
                    var response = req.CreateResponse(HttpStatusCode.Unauthorized);
                    response.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);
                    await response.WriteAsJsonAsync(new UnauthorizedResponse { Message = "Invalid token or user context." });
                    context.SendResponseAsync(response);
                    return;
                }
            }

            // Get the token from the authorization header
            var token = authorization?.First().Replace("Bearer ", "") ?? string.Empty;

            UserContext userContext;

            if (_isDebug)
            {
                // In debug mode, directly extract claims from the token
                userContext = GetUserContextFromDebugToken(token);
            }
            else
            {
                // In normal mode, validate the token and retrieve the user context
                userContext = GetUserContextFromToken(token);
            }

            if (userContext == null)
            {
                var response = req.CreateResponse(HttpStatusCode.Unauthorized);
                await response.WriteAsJsonAsync(new UnauthorizedResponse { Message = "Invalid token or user context." });
                context.SendResponseAsync(response);
                return;
            }

            // Store the user context in the request context for later use
            _userContextService.SetUserContext(userContext);

            await next(context);
        }

        private UserContext GetUserContextFromDebugToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // Parse the token without validation in debug mode
                if (tokenHandler.CanReadToken(token))
                {
                    var claimsPrincipal = tokenHandler.ReadJwtToken(token);

                    // Extract user information from the claims
                    string userId = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                    string userName = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
                    string userEmail = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
                    string userRole = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        return null;
                    }

                    // Create and return the user context
                    return new UserContext(userId, userName, null);
                }
            }
            catch (Exception ex)
            {
                // Handle token parsing exceptions
                _logger.LogError(ex, "Error parsing debug token.");
            }

            return null;
        }

        public static UserContext GetUserContextFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    // Set your validation parameters here (issuer, audience, etc.)
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = "tu_issuer",
                    ValidAudience = "tu_audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret_key_mockup")),
                    ClockSkew = TimeSpan.Zero // Opcionalmente, puedes ajustar el tiempo de tolerancia
                };

                // Validate and parse the token
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Extract user information from the claims
                string userId = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                string userName = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
                string userEmail = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
                string userRole = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                {
                    return null;
                }

                // Create and return the user context
                return new UserContext(userId, userName, null);
            }
            catch (Exception ex)
            {
                // Handle token validation exceptions
                // Log the error if needed
                return null;
            }
        }
    }
}
