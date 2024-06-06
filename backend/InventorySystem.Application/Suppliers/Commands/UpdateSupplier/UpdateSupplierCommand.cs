using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Suppliers.DTOs;
using MediatR;

namespace InventorySystem.Application.Suppliers.Commands.UpdateSupplier;

public record UpdateSupplierCommand : IRequest<Result<SupplierDto>>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
}