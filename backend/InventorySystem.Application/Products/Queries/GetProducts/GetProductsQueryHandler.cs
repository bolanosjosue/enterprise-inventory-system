using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Products.DTOs;
using InventorySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<PagedResult<ProductDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedResult<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Include(p => p.Movements.Where(m => !m.IsDeleted))
            .AsQueryable();

        // Filtro por búsqueda (SKU o Nombre)
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.ToLower();
            query = query.Where(p =>
                p.Sku.ToLower().Contains(searchTerm) ||
                p.Name.ToLower().Contains(searchTerm));
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status) &&
            Enum.TryParse<ProductStatus>(request.Status, out var status))
        {
            query = query.Where(p => p.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .OrderBy(p => p.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Mapear a DTOs
        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Cost = p.Cost,
            MinimumStock = p.MinimumStock,
            MaximumStock = p.MaximumStock,
            ImageUrl = p.ImageUrl,
            Status = p.Status.ToString(),
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name,
            SupplierId = p.SupplierId,
            SupplierName = p.Supplier?.Name,
            CurrentStock = p.GetCurrentStock(),
            IsLowStock = p.IsLowStock(),
            CreatedAt = p.CreatedAt
        }).ToList();

        // Filtro por stock bajo (después de mapear porque usa método del dominio)
        if (request.LowStock.HasValue && request.LowStock.Value)
        {
            productDtos = productDtos.Where(p => p.IsLowStock).ToList();
            totalCount = productDtos.Count;
        }

        var pagedResult = PagedResult<ProductDto>.Create(
            productDtos,
            totalCount,
            request.Page,
            request.PageSize);

        return Result.Success(pagedResult);
    }
}