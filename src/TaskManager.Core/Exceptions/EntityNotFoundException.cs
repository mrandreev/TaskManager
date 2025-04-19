namespace TaskManager.Core.Exceptions;

public class EntityNotFoundException(string message) : Exception(message)
{
}
