using Application.Common.Models.Responses;
using Application.Features.Carts.Commands.AddToCart;
using Application.Features.Carts.Commands.RemoveFromCart;
using Application.Features.Carts.Commands.UpdateCartItemQuantity;
using Application.Features.Carts.Queries.GetCart;
using FunctionAppApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace FunctionAppApi.Resources;

public class CartFunction : FunctionBase
{
    public CartFunction(ILogger<CategoryFunction> logger, IMediator mediator, IMapper mapper)
            : base(logger, mediator, mapper)
    {
    }

    [Function("GetCart")]
    [OpenApiOperation(operationId: "getCart", tags: new[] { "Cart" })]
    [OpenApiSecurity(schemeName: "bearer_auth",
        SecuritySchemeType.Http,
        Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Result<CartDto>), Description = "The OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ErrorResponse), Description = "Unauthorized request")]
    public async Task<HttpResponseData> GetCartAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cart")] HttpRequestData request)
    {
        _logger.LogInformation($"Call to {nameof(GetCartAsync)}");
        Result<CartDto> result = await _mediator.Send(new GetCartQuery());

        return await request.CreateHttpResponseAsync(result);
    }

    [Function("AddToCart")]
    [OpenApiOperation(operationId: "addToCart", tags: new[] { "Cart" })]
    [OpenApiRequestBody("application/json", typeof(AddToCartDto))]
    public async Task<HttpResponseData> AddToCart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cart/items")] HttpRequestData request)
    {
        _logger.LogInformation("AddToCart function processed a request.");

        var addToCartDto = await request.ReadFromJsonAsync<AddToCartDto>();
        var command = _mapper.Map<AddToCartCommand>(addToCartDto);
        var result = await _mediator.Send(command);

        return await request.CreateHttpResponseAsync(result);
    }

    [Function("RemoveFromCart")]
    [OpenApiOperation(operationId: "removeFromCart", tags: new[] { "Cart" })]
    [OpenApiParameter(name: "itemId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The itemId of the Product")]
    public async Task<HttpResponseData> RemoveFromCart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "cart/items/{itemId}")] HttpRequestData request, int itemId)

    {
        _logger.LogInformation("RemoveFromCart function processed a request.");

        var command = new RemoveFromCartCommand { ItemId = itemId };
        var result = await _mediator.Send(command);

        return await request.CreateHttpResponseAsync(result);
    }

    [Function("UpdateCartItemQuantity")]
    [OpenApiOperation(operationId: "updateCartItemQuantity", tags: new[] { "Cart" })]
    [OpenApiParameter(name: "itemId", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
    [OpenApiRequestBody("application/json", typeof(UpdateCartItemQuantityDto))]
    public async Task<HttpResponseData> UpdateCartItemQuantity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "cart/items/{itemId}")] HttpRequestData request, int itemId)
    {
        _logger.LogInformation("UpdateCartItemQuantity function processed a request.");

        var command = await request.ReadFromJsonAsync<UpdateCartItemQuantityCommand>();
        command.CartItemId = itemId;

        var result = await _mediator.Send(command);

        return await request.CreateHttpResponseAsync(result);
    }
}
