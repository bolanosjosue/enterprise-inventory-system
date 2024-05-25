using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Warehouses.DTOs;
using MediatR;

namespace InventorySystem.Application.Warehouses.Commands.CreateWarehouse;

public record CreateWarehouseCommand : IRequest<Result<WarehouseDto>>
{
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public int? MaxCapacity { get; init; }
}