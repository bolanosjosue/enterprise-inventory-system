using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Suppliers.DTOs;
using InventorySystem.Domain.Entities;
using MediatR;

namespace InventorySystem.Application.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Result<SupplierDto>>
{
    private readonly IRepository<Supplier> _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSupplierCommandHandler(
        IRepository<Supplier> supplierRepository,
        IUnitOfWork unitOfWork)
    {
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var taxIdExists = await _supplierRepository.ExistsAsync(
            s => s.TaxId == request.TaxId,
            cancellationToken);

        if (taxIdExists)
            return Result.Failure<SupplierDto>($"Supplier with Tax ID '{request.TaxId}' already exists");

        var supplier = Supplier.Create(
            request.Name,
            request.TaxId,
            request.Email,
            request.Phone,
            request.Address);

        await _supplierRepository.AddAsync(supplier, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            TaxId = supplier.TaxId,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            IsActive = supplier.IsActive,
            ProductCount = 0,
            CreatedAt = supplier.CreatedAt
        };

        return Result.Success(dto);
    }
}