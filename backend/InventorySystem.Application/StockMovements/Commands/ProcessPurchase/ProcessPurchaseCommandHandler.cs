using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.StockMovements.DTOs;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.StockMovements.Commands.ProcessPurchase;

public class ProcessPurchaseCommandHandler : IRequestHandler<ProcessPurchaseCommand, Result<StockMovementDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRepository<StockMovement> _movementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessPurchaseCommandHandler(
        IApplicationDbContext context,
        IRepository<StockMovement> movementRepository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _movementRepository = movementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<StockMovementDto>> Handle(ProcessPurchaseCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
            return Result.Failure<StockMovementDto>("Product not found");

        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == request.WarehouseId, cancellationToken);

        if (warehouse == null)
            return Result.Failure<StockMovementDto>("Warehouse not found");

        if (!warehouse.IsActive)
            return Result.Failure<StockMovementDto>("Warehouse is not active");

        if (request.SupplierId.HasValue)
        {
            var supplierExists = await _context.Suppliers
                .AnyAsync(s => s.Id == request.SupplierId.Value, cancellationToken);

            if (!supplierExists)
                return Result.Failure<StockMovementDto>("Supplier not found");
        }

        var movement = StockMovement.CreateEntry(
            request.ProductId,
            request.WarehouseId,
            request.Quantity,
            request.UnitPrice,
            request.Reference,
            request.Notes);

        await _movementRepository.AddAsync(movement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdMovement = await _context.StockMovements
            .Include(m => m.Product)
            .Include(m => m.Warehouse)
            .FirstAsync(m => m.Id == movement.Id, cancellationToken);

        var dto = new StockMovementDto
        {
            Id = createdMovement.Id,
            Type = createdMovement.Type.ToString(),
            Quantity = createdMovement.Quantity,
            UnitPrice = createdMovement.UnitPrice,
            TotalAmount = createdMovement.Quantity * createdMovement.UnitPrice,
            Reference = createdMovement.Reference,
            Notes = createdMovement.Notes,
            ProductId = createdMovement.ProductId,
            ProductName = createdMovement.Product.Name,
            ProductSku = createdMovement.Product.Sku,
            WarehouseId = createdMovement.WarehouseId,
            WarehouseName = createdMovement.Warehouse.Name,
            CreatedAt = createdMovement.CreatedAt,
            CreatedBy = createdMovement.CreatedBy
        };

        return Result.Success(dto);
    }
}