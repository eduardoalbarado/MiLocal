using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace FunctionAppApi.Extensions;

public static class HttpRequestDataExtensions
{
    public static async Task<HttpResponseData> CreateHttpResponseAsync(
        this HttpRequestData request, object data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var response = request.CreateResponse(statusCode);
        response.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);
        await response.WriteAsJsonAsync(data);

        return response;
    }
}
