using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Suppliers.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, Result<SupplierDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSupplierByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SupplierDto>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await _context.Suppliers
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (supplier == null)
            return Result.Failure<SupplierDto>("Supplier not found");

        var dto = new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            TaxId = supplier.TaxId,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            IsActive = supplier.IsActive,
            ProductCount = supplier.Products.Count,
            CreatedAt = supplier.CreatedAt
        };

        return Result.Success(dto);
    }
}