namespace TaskManager.Core.Exceptions;

public class ValidationException(string message) : Exception(message)
{
}
