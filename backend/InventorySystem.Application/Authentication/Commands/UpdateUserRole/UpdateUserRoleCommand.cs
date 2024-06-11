using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Authentication.Commands.UpdateUserRole;

public record UpdateUserRoleCommand : IRequest<Result<UserDto>>
{
    public Guid UserId { get; init; }
    public string Role { get; init; } = string.Empty;
}