using InventorySystem.Application.Common.Models;
using InventorySystem.Application.StockMovements.DTOs;
using MediatR;

namespace InventorySystem.Application.StockMovements.Queries.GetMovementHistory;

public record GetMovementHistoryQuery : IRequest<Result<PagedResult<StockMovementDto>>>
{
    public Guid? ProductId { get; init; }
    public Guid? WarehouseId { get; init; }
    public string? Type { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}