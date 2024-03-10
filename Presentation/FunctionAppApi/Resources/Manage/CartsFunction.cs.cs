using Application.Features.Carts.Queries.GetCarts;
using Application.Features.Carts.Queries.GetCart;

namespace FunctionAppApi.Resources.Manage;

public class CartsFunction : FunctionBase
{
    public CartsFunction(ILogger<CartsFunction> logger, IMediator mediator, IMapper mapper)
        : base(logger, mediator, mapper)
    {
    }
    
    [Function($"{nameof(GetsCart)}")]
    [OpenApiOperation(operationId: "getCart", tags: new[] { "Manage - Carts" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Application.Common.Models.Responses.Result<CartDto>), Description = "The OK response")]
    public async Task<IActionResult> GetsCart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "manage/carts")] HttpRequestData req)
    {
        _logger.LogInformation($"Call to {nameof(GetsCart)}");
        var result = await _mediator.Send(new GetCartsQuery());

        return new OkObjectResult(result);
    }
}
