using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Warehouses.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Warehouses.Queries.GetWarehouses;

public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, Result<List<WarehouseDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetWarehousesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<WarehouseDto>>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await _context.Warehouses
            .Include(w => w.Movements)
            .OrderBy(w => w.Name)
            .Select(w => new WarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Address = w.Address,
                MaxCapacity = w.MaxCapacity,
                IsActive = w.IsActive,
                TotalMovements = w.Movements.Count,
                CreatedAt = w.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(warehouses);
    }
}