using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
