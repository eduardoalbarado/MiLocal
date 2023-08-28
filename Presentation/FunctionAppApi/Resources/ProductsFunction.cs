using Application.Common.Models.Responses;
using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetProductById;
using Application.Features.Products.Queries.GetProducts;

namespace FunctionAppApi.Resources
{
    public class ProductsFunction : FunctionBase
    {
        public ProductsFunction(ILogger<ProductsFunction> logger, IMediator mediator, IMapper mapper)
            : base(logger, mediator, mapper)
        {
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
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The ID of the product")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ProductDto), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(NotFoundResponse), Description = "Product not found")]
        public async Task<IActionResult> GetProductById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"Call to {nameof(GetProductById)} with Id: {id}");
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });

            return new OkObjectResult(result);
        }

        [Function("AddProduct")]
        [OpenApiOperation(operationId: "AddProduct", tags: new[] { "Products" })]
        [OpenApiRequestBody("application/json", typeof(AddProductDto))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Result<ProductDto>), Description = "The OK response with added product details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ErrorResponse), Description = "Bad request when the product data is invalid.")]
        public async Task<IActionResult> AddProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequestData req)
        {
            _logger.LogInformation("AddProduct function processed a request.");

            var addProductDto = await req.ReadFromJsonAsync<AddProductDto>();
            var command = _mapper.Map<AddProductCommand>(addProductDto);
            var result = await _mediator.Send(command);

            return new OkObjectResult(result);
        }

        [OpenApiOperation(operationId: "DeleteProduct", tags: new[] { "Products" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The ID of the product")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Result<string>), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(NotFoundResponse), Description = "Product not found")]
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
        [OpenApiRequestBody("application/json", typeof(UpdateProductDto))]
        public async Task<IActionResult> UpdateProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"UpdateProduct function processed a request for Id: {id}");

            var updateProductDto = await req.ReadFromJsonAsync<UpdateProductDto>();
            var command = _mapper.Map<UpdateProductCommand>(updateProductDto);
            command.Id = id;

            var result = await _mediator.Send(command);

            return new OkObjectResult(result);
        }
    }
}
