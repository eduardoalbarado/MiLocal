using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetProducts;
using Application.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace FunctionAppApi.Resources
{
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
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequestData req)
        {
            _logger.LogInformation($"Call to {nameof(GetProducts)}");
            var result = await _mediator.Send(new GetProductsQuery());

            return new OkObjectResult(result);
        }

        [Function("GetProductById")]
        [OpenApiOperation(operationId: "GetProductById", tags: new[] { "Products" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetProductById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"Call to {nameof(GetProductById)} with Id: {id}");
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });

            return new OkObjectResult(result);
        }

        [Function("AddProduct")]
        [OpenApiOperation(operationId: "AddProduct", tags: new[] { "Products" })]
        [OpenApiRequestBody("application/json", typeof(AddProductCommand))]
        public async Task<IActionResult> AddProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequestData req)
        {
            _logger.LogInformation("AddProduct function processed a request.");

            var command = await req.ReadFromJsonAsync<AddProductCommand>();

            var result = await _mediator.Send(command);

            return new OkObjectResult(result);
        }

        [Function("DeleteProduct")]
        [OpenApiOperation(operationId: "DeleteProduct", tags: new[] { "Products" })]
        [OpenApiRequestBody("application/json", typeof(DeleteProductCommand))]
        public async Task<IActionResult> DeleteProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"DeleteProduct function processed a request for Id: {id}");

            var command = new DeleteProductCommand { Id = id };

            var result = await _mediator.Send(command);

            return new OkObjectResult(result);
        }

        [Function("UpdateProduct")]
        [OpenApiOperation(operationId: "UpdateProduct", tags: new[] { "Products" })]
        [OpenApiRequestBody("application/json", typeof(UpdateProductCommand))]
        public async Task<IActionResult> UpdateProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"UpdateProduct function processed a request for Id: {id}");

            var command = await req.ReadFromJsonAsync<UpdateProductCommand>();

            command.Id = id;

            var result = await _mediator.Send(command);

            return new OkObjectResult(result);
        }
    }
}
