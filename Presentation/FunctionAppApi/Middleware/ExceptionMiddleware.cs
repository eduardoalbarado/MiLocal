using Application.Common.Models.Responses;
using Application.Exceptions;
using FluentValidation;
using FunctionAppApi.Extensions;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Collections.Generic;

namespace FunctionAppApi.Middleware
{
    public class ExceptionMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                var errors = new Dictionary<string, string[]>();
                foreach (var failure in ex.Errors)
                {
                    if (!errors.ContainsKey(failure.PropertyName))
                    {
                        errors[failure.PropertyName] = new string[] { failure.ErrorMessage };
                    }
                    else
                    {
                        var existingErrors = errors[failure.PropertyName];
                        Array.Resize(ref existingErrors, existingErrors.Length + 1);
                        existingErrors[^1] = failure.ErrorMessage;
                        errors[failure.PropertyName] = existingErrors;
                    }
                }

                var response = new ValidationErrorResponse("Validation Error", errors);
                await context.CreateJsonResponse(HttpStatusCode.BadRequest, response);
            }
            catch (HttpResponseException ex)
            {
                var response = new ErrorResponse("Business Error", ex.Message, ex.StatusCode.ToString());
                await context.CreateJsonResponse(ex.StatusCode, response);
            }
            catch (NotFoundException ex)
            {
                var response = new ErrorResponse("Not Found", ex.Message, "RESOURCE_NOT_FOUND");
                await context.CreateJsonResponse(HttpStatusCode.NotFound, response);
            }
            catch (Exception ex)
            {
                var response = new ErrorResponse("Internal Server Error", "An unexpected error occurred.");
                await context.CreateJsonResponse(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
