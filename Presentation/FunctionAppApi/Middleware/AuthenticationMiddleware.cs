using Application.Interfaces;
using FunctionAppApi.Extensions;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Linq;

namespace FunctionAppApi.Middleware
{
    public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
    {
        private ILogger _logger;
        private readonly IUserContextService _userContextService;

        public AuthenticationMiddleware(IUserContextService userContextService)
        {
            _userContextService = userContextService;
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
                ////TODO: Add a bypass for Debugging or Loca running to allow work without token
                //var response = req.CreateResponse(HttpStatusCode.Unauthorized);
                //response.Headers.Add("Content-Type", "text/plain");
                //await response.WriteStringAsync("You must provide valid credentials to access this resource.");
                //context.SendResponseAsync(response);
                //return;
            }

            // Get the token from the authorization header
            var token = authorization?.First().Replace("Bearer ", "") ?? string.Empty;

            // TODO: Validate the token and retrieve the user context
            var userContext = GetUserContextFromToken(token);

            if (userContext == null)
            {
                var response = req.CreateResponse(HttpStatusCode.Unauthorized);
                response.Headers.Add("Content-Type", "text/plain");
                await response.WriteStringAsync("Invalid token or user context.");
                context.SendResponseAsync(response);
                return;
            }

            // Store the user context in the request context for later use
            _userContextService.SetUserContext(userContext);

            await next(context);
        }

        private UserContext GetUserContextFromToken(string token)
        {
            string userId;
            string userName;
            if (string.IsNullOrEmpty(token))
            {
                // Return mock data if the token is empty or null
                userId = "d2b5b44a-f39f-4e42-94c1-7b98e14a3ca8";
                userName = "John Doe";
                return new UserContext(userId, userName, null);
            }

            // TODO: Implement token validation and user context retrieval logic
            // Here, you can validate the token and extract the relevant user information

            // For simplicity, this implementation returns a dummy user context
            userId = "d2b5b44a-f39f-4e42-94c1-7b98e14a3ca8";
            userName = "John Doe";
            return new UserContext(userId, userName, null);
        }
    }
}
