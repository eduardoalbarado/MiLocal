using FunctionAppApi.Extensions;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionAppApi
{
    public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
    {
        private ILogger _logger;

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
            context.Items.Add("UserContext", userContext);

            await next(context);
        }

        private UserContext GetUserContextFromToken(string token)
        {
            string userId;
            string userName;
            if (string.IsNullOrEmpty(token))
            {
                // Return mock data if the token is empty or null
                userId = "123456";
                userName = "John Doe";
                return new UserContext(userId, userName);
            }

            // TODO: Implement token validation and user context retrieval logic
            // Here, you can validate the token and extract the relevant user information

            // For simplicity, this implementation returns a dummy user context
            userId = "123456";
            userName = "John Doe";
            return new UserContext(userId, userName);
        }
    }
}
