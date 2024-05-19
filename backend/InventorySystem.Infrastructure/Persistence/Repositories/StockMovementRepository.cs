using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Persistence.Repositories;

public class StockMovementRepository : GenericRepository<StockMovement>
{
    public StockMovementRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<StockMovement>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(m => m.ProductId == productId)
            .Include(m => m.Product)
            .Include(m => m.Warehouse)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockMovement>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(m => m.WarehouseId == warehouseId)
            .Include(m => m.Product)
            .Include(m => m.Warehouse)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockMovement>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(m => m.CreatedAt >= startDate && m.CreatedAt <= endDate)
            .Include(m => m.Product)
            .Include(m => m.Warehouse)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCurrentStockAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default)
    {
        var movements = await _dbSet
            .Where(m => m.ProductId == productId && m.WarehouseId == warehouseId && !m.IsDeleted)
            .ToListAsync(cancellationToken);

        return movements.Sum(m => m.Type == MovementType.Entry ? m.Quantity : -m.Quantity);
    }
}