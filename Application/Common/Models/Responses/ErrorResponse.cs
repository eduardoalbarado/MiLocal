namespace Application.Common.Models.Responses;
public class ErrorResponse
{
    public string Error { get; set; } = "GenericError";
    public string Message { get; set; } = "An error occurred while processing the request.";
    public ErrorResponse(string message)
    {
        Message = message;
    }

    public ErrorResponse(string error, string message)
    {
        Error = error;
        Message = message;
    }
}