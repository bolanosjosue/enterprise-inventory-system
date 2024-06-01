using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.StockMovements.DTOs;
using InventorySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.StockMovements.Queries.GetMovementHistory;

public class GetMovementHistoryQueryHandler : IRequestHandler<GetMovementHistoryQuery, Result<PagedResult<StockMovementDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetMovementHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedResult<StockMovementDto>>> Handle(GetMovementHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = _context.StockMovements
            .Include(m => m.Product)
            .Include(m => m.Warehouse)
            .Include(m => m.DestinationWarehouse)
            .AsQueryable();

        if (request.ProductId.HasValue)
        {
            query = query.Where(m => m.ProductId == request.ProductId.Value);
        }

        if (request.WarehouseId.HasValue)
        {
            query = query.Where(m =>
                m.WarehouseId == request.WarehouseId.Value ||
                m.DestinationWarehouseId == request.WarehouseId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Type) &&
            Enum.TryParse<MovementType>(request.Type, out var type))
        {
            query = query.Where(m => m.Type == type);
        }

        if (request.StartDate.HasValue)
        {
            query = query.Where(m => m.CreatedAt >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            var endOfDay = request.EndDate.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(m => m.CreatedAt <= endOfDay);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var movements = await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var movementDtos = movements.Select(m => new StockMovementDto
        {
            Id = m.Id,
            Type = m.Type.ToString(),
            Quantity = m.Quantity,
            UnitPrice = m.UnitPrice,
            TotalAmount = m.Quantity * m.UnitPrice,
            Reference = m.Reference,
            Notes = m.Notes,
            ProductId = m.ProductId,
            ProductName = m.Product.Name,
            ProductSku = m.Product.Sku,
            WarehouseId = m.WarehouseId,
            WarehouseName = m.Warehouse.Name,
            DestinationWarehouseId = m.DestinationWarehouseId,
            DestinationWarehouseName = m.DestinationWarehouse?.Name,
            CreatedAt = m.CreatedAt,
            CreatedBy = m.CreatedBy
        }).ToList();

        var pagedResult = PagedResult<StockMovementDto>.Create(
            movementDtos,
            totalCount,
            request.Page,
            request.PageSize);

        return Result.Success(pagedResult);
    }
}