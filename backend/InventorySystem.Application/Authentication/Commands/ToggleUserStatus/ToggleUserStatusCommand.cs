using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Authentication.Commands.ToggleUserStatus;

public record ToggleUserStatusCommand(Guid UserId) : IRequest<Result<UserDto>>;