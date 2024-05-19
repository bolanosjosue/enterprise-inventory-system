using InventorySystem.Application.Common.Interfaces;

namespace InventorySystem.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public string? UserId => "system";
    public string? Email => "system@inventory.com";
    public string? Role => "Admin";
    public bool IsAuthenticated => true;
}