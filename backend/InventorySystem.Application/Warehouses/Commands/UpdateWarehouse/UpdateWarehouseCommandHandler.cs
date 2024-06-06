using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Warehouses.DTOs;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Warehouses.Commands.UpdateWarehouse;

public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, Result<WarehouseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWarehouseCommandHandler(
        IApplicationDbContext context,
        IRepository<Warehouse> warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WarehouseDto>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(request.Id, cancellationToken);

        if (warehouse == null)
            return Result.Failure<WarehouseDto>("Warehouse not found");

        warehouse.UpdateInfo(request.Name, request.Address, request.MaxCapacity);

        _warehouseRepository.Update(warehouse);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var warehouseDto = new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Address = warehouse.Address,
            MaxCapacity = warehouse.MaxCapacity,
            IsActive = warehouse.IsActive,
            TotalMovements = await _context.StockMovements.CountAsync(m => m.WarehouseId == warehouse.Id, cancellationToken),
            CreatedAt = warehouse.CreatedAt
        };

        return Result.Success(warehouseDto);
    }
}