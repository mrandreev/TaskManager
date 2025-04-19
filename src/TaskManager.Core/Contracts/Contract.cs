using TaskManager.Core.Exceptions;

namespace TaskManager.Core.Validation;

public static class Contract
{
    public static void Requires(bool condition, string message)
    {
        if (!condition)
            throw new ValidationException(message);
    }

    public static void NotNull(object value, string paramName)
    {
        if (value == null)
            throw new ValidationException($"{paramName} cannot be null.");
    }

    public static void NotNullOrEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{paramName} cannot be null or empty.");
    }

    public static void MaxLength(string value, int maxLength, string paramName)
    {
        if (value != null && value.Length > maxLength)
            throw new ValidationException($"{paramName} cannot exceed {maxLength} characters.");
    }

    public static void EntityNotNull(object value, string entityName, object entityId)
    {
        if (value == null)
            throw new EntityNotFoundException($"{entityName} with id '{entityId}' was not found.");
    }
}
