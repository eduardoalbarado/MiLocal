using Application.Common.Models.Responses;
using Application.Exceptions;
using FluentValidation;
using FunctionAppApi.Extensions;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Collections.Generic;

namespace FunctionAppApi.Middleware;

public class ExceptionMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (ex.InnerException is ValidationException validationException)
            {
                var validationErrors = new List<ValidationError>();

                foreach (var error in validationException.Errors)
                {
                    var validationError = new ValidationError
                    {
                        PropertyName = error.PropertyName,
                        ErrorMessage = error.ErrorMessage
                    };

                    validationErrors.Add(validationError);
                }

                var errorResponse = new ValidationErrorResponse(validationErrors);

                await context.CreateJsonResponse(HttpStatusCode.BadRequest, errorResponse);
            }
            else if (ex.InnerException is BadRequestException badRequestException)
            {
                await context.CreateJsonResponse(HttpStatusCode.BadRequest, new ErrorResponse("Bad Request",badRequestException.Message));
            }
            else if (ex.InnerException is ConflictException conflictException)
            {
                await context.CreateJsonResponse(HttpStatusCode.Conflict, new ErrorResponse("Conflict",conflictException.Message));
            }
            else
            {
                // Handle general exception
                string message = $"An error occurred while processing the request.";

#if DEBUG
                message += $" Debug Message: {ex.Message}";
#endif
                var errorResponse = new ErrorResponse("General Exception", message);

                await context.CreateJsonResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
            return;
        }
    }
}