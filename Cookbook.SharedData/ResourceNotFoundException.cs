namespace Cookbook.SharedData;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException()
    {
    }

    public ResourceNotFoundException(Type type)
        : base(BuildMessage(type))
    {
    }

    public ResourceNotFoundException(Type type, string property, object value)
        : base(BuildMessage(type, property, value))
    {
    }

    public ResourceNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
    // public required Type RType { get; init; }
    // public required string RPropertyName { get; init; }
    // public required string RPropertyValue { get; init; }
    // public override string Message => $"Resource {RType.Name} not found.";

    private static string BuildMessage(Type resourceType, string? propertyName = null, object? propertyValue = null)
    {
        if (propertyValue is null || propertyName is null)
            return $"{resourceType.Name} Not Found";
        return $"{resourceType.Name} with {propertyName} of value {propertyValue} not found.";
    }
}