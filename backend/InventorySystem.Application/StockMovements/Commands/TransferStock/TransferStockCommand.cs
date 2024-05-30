using InventorySystem.Application.Common.Models;
using InventorySystem.Application.StockMovements.DTOs;
using MediatR;

namespace InventorySystem.Application.StockMovements.Commands.TransferStock;

public record TransferStockCommand : IRequest<Result<StockMovementDto>>
{
    public Guid ProductId { get; init; }
    public Guid SourceWarehouseId { get; init; }
    public Guid DestinationWarehouseId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public string Reference { get; init; } = string.Empty;
    public string? Notes { get; init; }
}