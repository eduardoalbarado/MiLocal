namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, object? entityId = null)
            : base($"The entity '{entityName}' with id '{entityId}' was not found.")
        {
        }
    }
}