using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Suppliers.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, Result<List<SupplierDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetSuppliersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<SupplierDto>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _context.Suppliers
            .Include(s => s.Products)
            .OrderBy(s => s.Name)
            .Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                TaxId = s.TaxId,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                IsActive = s.IsActive,
                ProductCount = s.Products.Count,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(suppliers);
    }
}