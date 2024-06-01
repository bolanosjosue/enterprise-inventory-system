using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.StockMovements.Queries.GetStockByWarehouse;

public record GetStockByWarehouseQuery(Guid WarehouseId) : IRequest<Result<List<ProductStockDto>>>;

public class ProductStockDto
{
    public Guid ProductId { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public decimal AverageCost { get; set; }
    public decimal TotalValue { get; set; }
}