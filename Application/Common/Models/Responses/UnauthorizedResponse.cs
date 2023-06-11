namespace Application.Common.Models.Responses;
public class UnauthorizedResponse
{
    public string Error { get; set; } = "Unauthorized";
    public string Message { get; set; } = "Access denied. User is not authorized.";
}
