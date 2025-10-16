namespace Cookbook.SharedData.Exceptions;

public class DuplicatePropertyException(string propertyName, string propertyValue)
    : Exception($"{propertyName} '{propertyValue}' is already in use")
{
    public string PropertyName { get; } = propertyName;
    public string PropertyValue { get; } = propertyValue;
}