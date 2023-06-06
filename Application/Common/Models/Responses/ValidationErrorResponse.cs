namespace Application.Common.Models.Responses;
public class ValidationErrorResponse
{
    public string Error { get; set; } = "ValidationError";
    public string Message { get; set; } = "One or more validation errors occurred.";
    public List<ValidationError> ValidationErrors { get; set; }
    public ValidationErrorResponse(List<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }
    public ValidationErrorResponse(string message, List<ValidationError> validationErrors)
    {
        Message = message;
        ValidationErrors = validationErrors;
    }

}

public class ValidationError
{
    /// <summary>
    /// The name of the property.
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// The error message
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// The property value that caused the failure.
    /// </summary>
}