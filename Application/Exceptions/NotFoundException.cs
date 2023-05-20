namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, int entityId)
            : base($"The entity '{entityName}' with id '{entityId}' was not found.")
        {
        }
    }
}