using Application.Features.Carts.Commands.AddToCart;
using Application.Features.Carts.Commands.RemoveFromCart;
using Application.Features.Carts.Commands.UpdateCartItemQuantity;
using Application.Features.Carts.Queries.GetCart;

namespace FunctionAppApi.Resources;

public class CartFunction : FunctionBase
{
    public CartFunction(ILogger<CartFunction> logger, IMediator mediator, IMapper mapper)
        : base(logger, mediator, mapper)
    {
    }

    [Function("GetCart")]
    [OpenApiOperation(operationId: "getCart", tags: new[] { "Cart" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> GetCart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cart")] HttpRequestData req)
    {
        _logger.LogInformation($"Call to {nameof(GetCart)}");
        var result = await _mediator.Send(new GetCartQuery());

        return new OkObjectResult(result);
    }

    [Function("AddToCart")]
    [OpenApiOperation(operationId: "addToCart", tags: new[] { "Cart" })]
    [OpenApiRequestBody("application/json", typeof(AddToCartDto))]
    public async Task<IActionResult> AddToCart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cart/items")] HttpRequestData req)
    {
        _logger.LogInformation("AddToCart function processed a request.");

        var addToCartDto = await req.ReadFromJsonAsync<AddToCartDto>();
        var command = _mapper.Map<AddToCartCommand>(addToCartDto);
        var result = await _mediator.Send(command);

        return new OkObjectResult(result);
    }

    [Function("RemoveFromCart")]
    [OpenApiOperation(operationId: "removeFromCart", tags: new[] { "Cart" })]
    public async Task<IActionResult> RemoveFromCart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "cart/items/{itemId}")] HttpRequestData req, int itemId)
    {
        _logger.LogInformation("RemoveFromCart function processed a request.");

        var command = new RemoveFromCartCommand { ItemId = itemId };
        var result = await _mediator.Send(command);

        return new OkObjectResult(result);
    }

    [Function("UpdateCartItemQuantity")]
    [OpenApiOperation(operationId: "updateCartItemQuantity", tags: new[] { "Cart" })]
    [OpenApiParameter(name: "itemId", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
    [OpenApiRequestBody("application/json", typeof(UpdateCartItemQuantityDto))]
    public async Task<IActionResult> UpdateCartItemQuantity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "cart/items/{itemId}")] HttpRequestData req, int itemId)
    {
        _logger.LogInformation("UpdateCartItemQuantity function processed a request.");

        var command = await req.ReadFromJsonAsync<UpdateCartItemQuantityCommand>();
        command.CartItemId = itemId;

        var result = await _mediator.Send(command);

        return new OkObjectResult(result);
    }
}
