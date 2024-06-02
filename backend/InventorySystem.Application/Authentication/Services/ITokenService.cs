using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Authentication.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}