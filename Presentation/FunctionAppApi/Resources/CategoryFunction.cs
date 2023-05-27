using Application.Features.Categories.Commands.AddCategoryCommand;
using Application.Features.Categories.Commands.DeleteCategoryCommand;
using Application.Features.Categories.Commands.UpdateCategoryCommand;
using Application.Features.Categories.Queries.GetCategories;
using Application.Features.Categories.Queries.GetCategoryById;

namespace FunctionAppApi.Resources
{
    public class CategoryFunction : FunctionBase
    {

        public CategoryFunction(ILogger<CategoryFunction> logger, IMediator mediator, IMapper mapper)
            : base(logger, mediator, mapper)
        {
        }

        [Function("GetCategories")]
        [OpenApiOperation(operationId: "categories", tags: new[] { "Categories" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetCategories(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "categories")] HttpRequestData req)
        {
            _logger.LogInformation($"Call to {nameof(GetCategories)}");

            var result = await _mediator.Send(new GetCategoriesQuery());
            return new OkObjectResult(result);
        }

        [Function("GetCategoryById")]
        [OpenApiOperation(operationId: "GetCategoryById", tags: new[] { "Categories" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetCategoryById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "categories/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"Call to {nameof(GetCategoryById)} with Id: {id}");

            var result = await _mediator.Send(new GetCategoryByIdQuery { Id = id });
            return new OkObjectResult(result);
        }

        [Function("AddCategory")]
        [OpenApiOperation(operationId: "AddCategory", tags: new[] { "Categories" })]
        [OpenApiRequestBody("application/json", typeof(AddCategoryDto))]
        public async Task<IActionResult> AddCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "categories")] HttpRequestData req)
        {
            _logger.LogInformation("AddCategory function processed a request.");

            var addCategoryDto = await req.ReadFromJsonAsync<AddCategoryDto>();
            var command = _mapper.Map<AddCategoryCommand>(addCategoryDto);
            var result = await _mediator.Send(command);
            return new OkObjectResult(result);
        }

        [Function("DeleteCategory")]
        [OpenApiOperation(operationId: "DeleteCategory", tags: new[] { "Categories" })]
        public async Task<IActionResult> DeleteCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "categories/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"DeleteCategory function processed a request for Id: {id}");

            var command = new DeleteCategoryCommand { Id = id };
            var result = await _mediator.Send(command);
            return new OkObjectResult(result);
        }

        [Function("UpdateCategory")]
        [OpenApiOperation(operationId: "UpdateCategory", tags: new[] { "Categories" })]
        [OpenApiRequestBody("application/json", typeof(UpdateCategoryDto))]
        public async Task<IActionResult> UpdateCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "categories/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"UpdateCategory function processed a request for Id: {id}");

            var updateCategoryDto = await req.ReadFromJsonAsync<UpdateCategoryDto>();
            var command = _mapper.Map<UpdateCategoryCommand>(updateCategoryDto);
            command.Id = id;
            var result = await _mediator.Send(command);

            return new OkObjectResult(result);
        }
    }
}
