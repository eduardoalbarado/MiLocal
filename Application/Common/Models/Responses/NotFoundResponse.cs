namespace Application.Common.Models.Responses;
public class NotFoundResponse
{
    public string Error { get; set; } = "NotFound";
    public string Message { get; set; } = "The requested resource was not found.";
    public string ResourceId { get; set; } = string.Empty;
}