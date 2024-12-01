using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Any;
using System.Collections.Generic;

namespace FunctionAppApi.OpenApi;

public class InternalErrorResponseDocumentFilter : IDocumentFilter
{
    public void Apply(IHttpRequestDataObject req, OpenApiDocument document)
    {
        // Agrega el esquema errorResponse a components.schemas si no está presente
        if (!document.Components.Schemas.ContainsKey("internalErrorResponse"))
        {
            document.Components.Schemas["internalErrorResponse"] = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["title"] = new OpenApiSchema { Type = "string" },
                    ["detail"] = new OpenApiSchema { Type = "string" },
                    ["errorCode"] = new OpenApiSchema { Type = "string", Nullable = true }
                },
                Example = new OpenApiObject
                {
                    ["title"] = new OpenApiString("Database Error"),
                    ["detail"] = new OpenApiString("Unable to connect to the database. Please try again later."),
                    ["errorCode"] = new OpenApiString(string.Empty)
                }
            };
        }

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
                            Id = ToCamelCase("InternalErrorResponse")
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

