using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Authentication.Commands.Login;

public record LoginCommand : IRequest<Result<LoginDto>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}