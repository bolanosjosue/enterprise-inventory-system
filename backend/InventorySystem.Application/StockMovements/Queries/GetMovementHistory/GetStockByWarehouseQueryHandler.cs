using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.StockMovements.Queries.GetStockByWarehouse;

public class GetStockByWarehouseQueryHandler : IRequestHandler<GetStockByWarehouseQuery, Result<List<ProductStockDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetStockByWarehouseQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ProductStockDto>>> Handle(GetStockByWarehouseQuery request, CancellationToken cancellationToken)
    {
        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == request.WarehouseId, cancellationToken);

        if (warehouse == null)
            return Result.Failure<List<ProductStockDto>>("Warehouse not found");

        var movements = await _context.StockMovements
            .Where(m => m.WarehouseId == request.WarehouseId && !m.IsDeleted)
            .Include(m => m.Product)
            .ToListAsync(cancellationToken);

        var stockByProduct = movements
            .GroupBy(m => new { m.ProductId, m.Product.Sku, m.Product.Name })
            .Select(g => new ProductStockDto
            {
                ProductId = g.Key.ProductId,
                ProductSku = g.Key.Sku,
                ProductName = g.Key.Name,
                CurrentStock = g.Sum(m => m.Type == MovementType.Entry ? m.Quantity : -m.Quantity),
                AverageCost = g.Any() ? g.Average(m => m.UnitPrice) : 0,
                TotalValue = g.Sum(m => (m.Type == MovementType.Entry ? m.Quantity : -m.Quantity) * m.UnitPrice)
            })
            .Where(s => s.CurrentStock > 0)
            .OrderBy(s => s.ProductName)
            .ToList();

        return Result.Success(stockByProduct);
    }
}