using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Warehouses.DTOs;
using InventorySystem.Domain.Entities;
using MediatR;

namespace InventorySystem.Application.Warehouses.Commands.CreateWarehouse;

public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Result<WarehouseDto>>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWarehouseCommandHandler(
        IRepository<Warehouse> warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await _warehouseRepository.ExistsAsync(
            w => w.Name.ToLower() == request.Name.ToLower(),
            cancellationToken);

        if (nameExists)
            return Result.Failure<WarehouseDto>($"Warehouse '{request.Name}' already exists");

        var warehouse = Warehouse.Create(
            request.Name,
            request.Address,
            request.MaxCapacity);

        await _warehouseRepository.AddAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Address = warehouse.Address,
            MaxCapacity = warehouse.MaxCapacity,
            IsActive = warehouse.IsActive,
            TotalMovements = 0,
            CreatedAt = warehouse.CreatedAt
        };

        return Result.Success(dto);
    }
}