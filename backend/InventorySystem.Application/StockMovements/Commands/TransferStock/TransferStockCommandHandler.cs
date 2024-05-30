using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.StockMovements.DTOs;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.StockMovements.Commands.TransferStock;

public class TransferStockCommandHandler : IRequestHandler<TransferStockCommand, Result<StockMovementDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRepository<StockMovement> _movementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferStockCommandHandler(
        IApplicationDbContext context,
        IRepository<StockMovement> movementRepository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _movementRepository = movementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<StockMovementDto>> Handle(TransferStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
            return Result.Failure<StockMovementDto>("Product not found");

        if (product.Status != ProductStatus.Active)
            return Result.Failure<StockMovementDto>("Product is not active");

        var sourceWarehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == request.SourceWarehouseId, cancellationToken);

        if (sourceWarehouse == null)
            return Result.Failure<StockMovementDto>("Source warehouse not found");

        if (!sourceWarehouse.IsActive)
            return Result.Failure<StockMovementDto>("Source warehouse is not active");

        var destinationWarehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == request.DestinationWarehouseId, cancellationToken);

        if (destinationWarehouse == null)
            return Result.Failure<StockMovementDto>("Destination warehouse not found");

        if (!destinationWarehouse.IsActive)
            return Result.Failure<StockMovementDto>("Destination warehouse is not active");

        var movements = await _context.StockMovements
            .Where(m => m.ProductId == request.ProductId &&
                       m.WarehouseId == request.SourceWarehouseId &&
                       !m.IsDeleted)
            .ToListAsync(cancellationToken);

        var currentStock = movements.Sum(m => m.Type == MovementType.Entry ? m.Quantity : -m.Quantity);

        if (currentStock < request.Quantity)
        {
            return Result.Failure<StockMovementDto>(
                $"Insufficient stock in source warehouse. Available: {currentStock}, Requested: {request.Quantity}");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var movement = StockMovement.CreateTransfer(
                request.ProductId,
                request.SourceWarehouseId,
                request.DestinationWarehouseId,
                request.Quantity,
                request.UnitPrice,
                request.Reference,
                request.Notes);

            await _movementRepository.AddAsync(movement, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var createdMovement = await _context.StockMovements
                .Include(m => m.Product)
                .Include(m => m.Warehouse)
                .Include(m => m.DestinationWarehouse)
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
                DestinationWarehouseId = createdMovement.DestinationWarehouseId,
                DestinationWarehouseName = createdMovement.DestinationWarehouse?.Name,
                CreatedAt = createdMovement.CreatedAt,
                CreatedBy = createdMovement.CreatedBy
            };

            return Result.Success(dto);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}