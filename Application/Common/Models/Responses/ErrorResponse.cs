namespace Application.Common.Models.Responses;
public class ErrorResponse
{
    public string Title { get; set; }
    public string Detail { get; set; }
    public string ErrorCode { get; set; }

    public ErrorResponse(string title, string detail, string errorCode = null)
    {
        Title = title;
        Detail = detail;
        ErrorCode = errorCode;
    }
}