namespace InventorySystem.Infrastructure.Identity;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpirationInHours { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}