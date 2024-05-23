using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Products.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Include(p => p.Movements.Where(m => !m.IsDeleted))
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
        {
            return Result.Failure<ProductDto>("Product not found");
        }

        var productDto = new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Cost = product.Cost,
            MinimumStock = product.MinimumStock,
            MaximumStock = product.MaximumStock,
            ImageUrl = product.ImageUrl,
            Status = product.Status.ToString(),
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            SupplierId = product.SupplierId,
            SupplierName = product.Supplier?.Name,
            CurrentStock = product.GetCurrentStock(),
            IsLowStock = product.IsLowStock(),
            CreatedAt = product.CreatedAt
        };

        return Result.Success(productDto);
    }
}