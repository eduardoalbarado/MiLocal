using Application.Common.Models.Responses;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Any;
using System.Collections.Generic;

namespace FunctionAppApi.OpenApi;

public class UnauthorizedResponseDocumentFilter : IDocumentFilter
{
    public void Apply(IHttpRequestDataObject req, OpenApiDocument document)
    {
        // Agrega el esquema unauthorizedResponse a components.schemas si no está presente
        if (!document.Components.Schemas.ContainsKey("unauthorizedResponse"))
        {
            document.Components.Schemas["unauthorizedResponse"] = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["error"] = new OpenApiSchema { Type = "string" },
                    ["message"] = new OpenApiSchema { Type = "string" }
                },
                Example = new OpenApiObject
                {
                    ["error"] = new OpenApiString("Unauthorized"),
                    ["message"] = new OpenApiString("Access denied. User is not authorized.")
                }
            };
        }

        // Define the response schema for Unauthorized
        var unauthorizedResponse = new OpenApiResponse
        {
            Description = "Unauthorized request",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = ToCamelCase(nameof(UnauthorizedResponse))
                        }
                    }
                }
            }
        };

        // Add the Unauthorized response to all operations in the document
        foreach (var path in document.Paths.Values)
        {
            foreach (var operation in path.Operations.Values)
            {
                operation.Responses["401"] = unauthorizedResponse;
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
