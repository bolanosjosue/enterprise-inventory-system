using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Warehouses.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Warehouses.Queries.GetWarehouseById;

public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, Result<WarehouseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWarehouseByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<WarehouseDto>> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        var warehouse = await _context.Warehouses
            .Include(w => w.Movements)
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (warehouse == null)
            return Result.Failure<WarehouseDto>("Warehouse not found");

        var dto = new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Address = warehouse.Address,
            MaxCapacity = warehouse.MaxCapacity,
            IsActive = warehouse.IsActive,
            TotalMovements = warehouse.Movements.Count,
            CreatedAt = warehouse.CreatedAt
        };

        return Result.Success(dto);
    }
}