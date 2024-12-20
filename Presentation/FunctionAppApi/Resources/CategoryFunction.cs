﻿using Application.Common.Models.Responses;
using Application.Features.Categories.Commands.AddCategoryCommand;
using Application.Features.Categories.Commands.DeleteCategoryCommand;
using Application.Features.Categories.Commands.UpdateCategoryCommand;
using Application.Features.Categories.Queries.GetCategories;
using Application.Features.Categories.Queries.GetCategoryById;
using Azure.Core;
using FunctionAppApi.Extensions;

namespace FunctionAppApi.Resources;

public class CategoryFunction : FunctionBase
{

    public CategoryFunction(ILogger<CategoryFunction> logger, IMediator mediator, IMapper mapper)
        : base(logger, mediator, mapper)
    {
    }

    [Function("GetCategories")]
    [OpenApiOperation(operationId: "categories", tags: new[] { "Categories" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public async Task<HttpResponseData> GetCategories(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "categories")] HttpRequestData req)
    {
        _logger.LogInformation($"Call to {nameof(GetCategories)}");

        var result = await _mediator.Send(new GetCategoriesQuery());
        return await CreateJsonResponseAsync(req, result);
    }

    [Function("GetCategoryById")]
    [OpenApiOperation(operationId: "GetCategoryById", tags: new[] { "Categories" })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The ID of the category")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(NotFoundResponse), Description = "Category not found")]
    public async Task<HttpResponseData> GetCategoryById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "categories/{id}")] HttpRequestData req, int id)
    {
        _logger.LogInformation($"Call to {nameof(GetCategoryById)} with Id: {id}");

        var result = await _mediator.Send(new GetCategoryByIdQuery { Id = id });
        return await CreateJsonResponseAsync(req, result);
    }

    [Function("AddCategory")]
    [OpenApiOperation(operationId: "AddCategory", tags: new[] { "Categories" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Result<int>), Description = "The OK response")]
    [OpenApiRequestBody("application/json", typeof(AddCategoryDto))]
    public async Task<HttpResponseData> AddCategory(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "categories")] HttpRequestData request)
    {
        _logger.LogInformation("AddCategory function processed a request.");

        var addCategoryDto = await request.ReadFromJsonAsync<AddCategoryDto>();
        var command = _mapper.Map<AddCategoryCommand>(addCategoryDto);
        var result = await _mediator.Send(command);

        return await request.CreateHttpResponseAsync(result);
    }

    [Function("DeleteCategory")]
    [OpenApiOperation(operationId: "DeleteCategory", tags: new[] { "Categories" })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The ID of the category")]
    public async Task<HttpResponseData> DeleteCategory(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "categories/{id}")] HttpRequestData req, int id)
    {
        _logger.LogInformation($"DeleteCategory function processed a request for Id: {id}");

        var command = new DeleteCategoryCommand { Id = id };
        var result = await _mediator.Send(command);
        return await CreateJsonResponseAsync(req, result);
    }

    [Function("UpdateCategory")]
    [OpenApiOperation(operationId: "UpdateCategory", tags: new[] { "Categories" })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The ID of the category")]
    [OpenApiRequestBody("application/json", typeof(UpdateCategoryDto))]
    public async Task<HttpResponseData> UpdateCategory(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "categories/{id}")] HttpRequestData req, int id)
    {
        _logger.LogInformation($"UpdateCategory function processed a request for Id: {id}");

        var updateCategoryDto = await req.ReadFromJsonAsync<UpdateCategoryDto>();
        var command = _mapper.Map<UpdateCategoryCommand>(updateCategoryDto);
        command.Id = id;
        var result = await _mediator.Send(command);

        return await CreateJsonResponseAsync(req, result);
    }
}
