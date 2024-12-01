using Application.Common.Models.Checkout;
using Application.Common.Models.Responses;
using Application.Features.Checkout.Commands.StartCheckout;
using FunctionAppApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace FunctionAppApi.Resources;

public class CheckoutFunction : FunctionBase
{
    public CheckoutFunction(ILogger<CheckoutFunction> logger, IMediator mediator, IMapper mapper)
        : base(logger, mediator, mapper)
    {
    }

    [Function("StartCheckout")]
    [OpenApiOperation(operationId: "startCheckout", tags: new[] { "Checkout" })]
    [OpenApiSecurity(schemeName: "bearer_auth",
        SecuritySchemeType.Http,
        Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiRequestBody("application/json", typeof(StartCheckoutRequestDto), Description = "Request data for initiating checkout")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Result<CheckoutResponseDto>), Description = "Checkout initiated successfully")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ErrorResponse), Description = "Bad request")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ErrorResponse), Description = "Internal server error")]
    public async Task<HttpResponseData> StartCheckoutAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "checkout")] HttpRequestData request)
    {
        _logger.LogInformation("StartCheckout function processed a request.");

        var checkoutRequestDto = await request.ReadFromJsonAsync<StartCheckoutRequestDto>();
        var command = _mapper.Map<StartCheckoutCommand>(checkoutRequestDto);
        var result = await _mediator.Send(command);

        return await request.CreateHttpResponseAsync(result);
    }
}
