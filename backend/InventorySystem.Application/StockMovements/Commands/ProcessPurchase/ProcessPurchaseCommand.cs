using InventorySystem.Application.Common.Models;
using InventorySystem.Application.StockMovements.DTOs;
using MediatR;

namespace InventorySystem.Application.StockMovements.Commands.ProcessPurchase;

public record ProcessPurchaseCommand : IRequest<Result<StockMovementDto>>
{
    public Guid ProductId { get; init; }
    public Guid WarehouseId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public string Reference { get; init; } = string.Empty;
    public Guid? SupplierId { get; init; }
    public string? Notes { get; init; }
}