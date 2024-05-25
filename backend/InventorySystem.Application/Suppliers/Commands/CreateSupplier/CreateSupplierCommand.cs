using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Suppliers.DTOs;
using MediatR;

namespace InventorySystem.Application.Suppliers.Commands.CreateSupplier;

public record CreateSupplierCommand : IRequest<Result<SupplierDto>>
{
    public string Name { get; init; } = string.Empty;
    public string TaxId { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
}