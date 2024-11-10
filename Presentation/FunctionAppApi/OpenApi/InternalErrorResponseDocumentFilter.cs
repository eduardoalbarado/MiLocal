using Application.Common.Models.Responses;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using System.Collections.Generic;

namespace FunctionAppApi.OpenApi;

public class InternalErrorResponseDocumentFilter : IDocumentFilter
{
    public void Apply(IHttpRequestDataObject req, OpenApiDocument document)
    {
        // Define the response schema for Internal Server Error
        var internalServerErrorResponse = new OpenApiResponse
        {
            Description = "Internal Error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = ToCamelCase(nameof(ErrorResponse))
                        }
                    }
                }
            }
        };

        // Add the Internal Server Error response to all operations in the document
        foreach (var path in document.Paths.Values)
        {
            foreach (var operation in path.Operations.Values)
            {
                operation.Responses["500"] = internalServerErrorResponse;
            }
        }
    }
    private string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
            return input;

        string camelCase = char.ToLower(input[0]) + input.Substring(1);
        return camelCase;
    }

}

