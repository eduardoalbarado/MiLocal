using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;

namespace FunctionAppApi.OpenApi;

public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
    private const string UriString = "https://MiLocal.com/support";
    private const string DescriptionString = "HTTP APIs that run on Azure Functions using OpenAPI specification.";

    public OpenApiInfo Info { get; set; } = new OpenApiInfo
    {
        Title = "MiLocal API on Azure Function",
        Version = "1.0.0",
        Description = DescriptionString,
        Contact = new OpenApiContact
        {
            Name = "MiLocal",
            Url = new Uri(UriString)
        }
    };

    public List<OpenApiServer> Servers { get; set; } = new();
    public OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
    public bool IncludeRequestingHostName { get; set; } = false;
    public bool ForceHttp { get; set; } = false;
    public bool ForceHttps { get; set; } = false;
    public List<IDocumentFilter> DocumentFilters { get; set; } = new List<IDocumentFilter>
    {
        new SecurityRequirementsDocumentFilter(),
        new InternalErrorResponseDocumentFilter(),
        new UnauthorizedResponseDocumentFilter()
    };

    //// Definir el esquema unauthorizedResponse con el ejemplo corregido
    //public IDictionary<string, OpenApiSchema> Schemas { get; set; } = new Dictionary<string, OpenApiSchema>
    //{
    //    ["unauthorizedResponse"] = new OpenApiSchema
    //    {
    //        Type = "object",
    //        Properties = new Dictionary<string, OpenApiSchema>
    //        {
    //            ["error"] = new OpenApiSchema { Type = "string" },
    //            ["message"] = new OpenApiSchema { Type = "string" }
    //        },
    //        Example = new OpenApiObject
    //        {
    //            ["error"] = new OpenApiString("Unauthorized"),
    //            ["message"] = new OpenApiString("Access denied. User is not authorized.")
    //        }
    //    }
    //};
}

public class SecurityRequirementsDocumentFilter : IDocumentFilter
{
    public void Apply(IHttpRequestDataObject req, OpenApiDocument document)
    {
        // Define the security scheme to be used
        var securityScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer_auth" }
        };

        // Add the security requirement to all operations in the document
        var securityRequirement = new OpenApiSecurityRequirement
        {
            [securityScheme] = new List<string>()
        };
        document.SecurityRequirements.Add(securityRequirement);
    }
}
