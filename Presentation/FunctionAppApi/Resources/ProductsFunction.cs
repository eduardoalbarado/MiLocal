using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace FunctionAppApi.Resources;

public class ProductsFunction
{
    private readonly ILogger<ProductsFunction> _logger;
    private readonly IMediator _mediator;

    public ProductsFunction(ILogger<ProductsFunction> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Function("GetProducts")]
    [OpenApiOperation(operationId: "products", tags: new[] { "Products" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> GetProducts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequestData req)
    {
        _logger.LogInformation($"Call to {nameof(GetProducts)}");
        var result = await _mediator.Send(new GetProductsQuery { });

        return new OkObjectResult(result);
    }

    [Function("AddProduct")]
    [OpenApiOperation(operationId: "AddProducts", tags: new[] { "Products" })]
    [OpenApiRequestBody("application/json", typeof(AddProductCommand))]

    public async Task<IActionResult> AddProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequestData req)
    {
        _logger.LogInformation("AddProduct function processed a request.");

        // Deserialize the request body to a AddProductCommand object
        var command = await req.ReadFromJsonAsync<AddProductCommand>();

        // Invoke the corresponding handler for the AddProductCommand
        var result = await _mediator.Send(command);

        return new OkObjectResult(result);
    }

}

