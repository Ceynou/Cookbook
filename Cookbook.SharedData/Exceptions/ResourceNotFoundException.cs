namespace Cookbook.SharedData.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(Type type)
        : base(BuildMessage(type))
    {
    }

    public ResourceNotFoundException(Type type, string property, object value)
        : base(BuildMessage(type, property, value))
    {
    }

    private static string BuildMessage(Type resourceType, string? propertyName = null, object? propertyValue = null)
    {
        if (propertyValue is null || propertyName is null)
            return $"{resourceType.Name} not found";
        return $"{resourceType.Name} with {propertyName} {propertyValue} not found.";
    }
}