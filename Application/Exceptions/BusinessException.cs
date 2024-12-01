using System.Net;

namespace Application.Exceptions;

public class BusinessException : Exception
{
    public string Message { get; }
    public HttpStatusCode StatusCode { get; }

    public BusinessException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message)
    {
        Message = message;
        StatusCode = statusCode;
    }
}
