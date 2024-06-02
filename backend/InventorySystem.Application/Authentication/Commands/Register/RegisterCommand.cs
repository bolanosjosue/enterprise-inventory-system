using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Authentication.Commands.Register;

public record RegisterCommand : IRequest<Result<LoginDto>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
}