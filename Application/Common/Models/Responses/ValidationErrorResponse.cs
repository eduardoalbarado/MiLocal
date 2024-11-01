namespace Application.Common.Models.Responses;
public class ValidationErrorResponse : ErrorResponse
{
    public Dictionary<string, string[]> Errors { get; set; }

    public ValidationErrorResponse(string title, Dictionary<string, string[]> errors)
        : base(title, "Validation failed for one or more properties.")
    {
        Errors = errors;
    }
}