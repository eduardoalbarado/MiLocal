using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions;

public class HttpResponseException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public object ErrorContent { get; }

    public HttpResponseException(HttpStatusCode statusCode, object errorContent = null)
        : base(errorContent?.ToString())
    {
        StatusCode = statusCode;
        ErrorContent = errorContent;
    }
}

