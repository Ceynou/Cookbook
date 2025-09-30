namespace Cookbook.SharedData;

public interface IJwtSettings
{
    public string Secret { get; }
    public string Issuer { get; }
    public string Audience { get; }
    public int ExpirationMinutes { get; }
}