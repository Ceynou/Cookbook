namespace Cookbook.SharedData;

public sealed record ErrorResponse
{
    public required string Error { get; set; }
    public required string Details { get; set; }
}