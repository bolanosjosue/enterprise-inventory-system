using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Warehouses.DTOs;
using MediatR;

namespace InventorySystem.Application.Warehouses.Commands.UpdateWarehouse;

public record UpdateWarehouseCommand : IRequest<Result<WarehouseDto>>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public int? MaxCapacity { get; init; }
}