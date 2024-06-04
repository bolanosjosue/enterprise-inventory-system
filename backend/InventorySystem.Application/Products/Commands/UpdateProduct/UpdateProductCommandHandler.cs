using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Products.DTOs;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IApplicationDbContext context,
        IRepository<Product> productRepository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
            return Result.Failure<ProductDto>("Product not found");

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
            return Result.Failure<ProductDto>("Category not found");

        if (request.SupplierId.HasValue)
        {
            var supplierExists = await _context.Suppliers
                .AnyAsync(s => s.Id == request.SupplierId.Value, cancellationToken);
            if (!supplierExists)
                return Result.Failure<ProductDto>("Supplier not found");
        }

        product.UpdateInfo(
            request.Name,
            request.Description,
            request.MinimumStock,
            request.MaximumStock,
            request.CategoryId,
            request.SupplierId,
            request.ImageUrl);

        product.UpdatePrice(request.Price, request.Cost);

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedProduct = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstAsync(p => p.Id == product.Id, cancellationToken);

        var productDto = new ProductDto
        {
            Id = updatedProduct.Id,
            Sku = updatedProduct.Sku,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Price = updatedProduct.Price,
            Cost = updatedProduct.Cost,
            MinimumStock = updatedProduct.MinimumStock,
            MaximumStock = updatedProduct.MaximumStock,
            ImageUrl = updatedProduct.ImageUrl,
            Status = updatedProduct.Status.ToString(),
            CategoryId = updatedProduct.CategoryId,
            CategoryName = updatedProduct.Category.Name,
            SupplierId = updatedProduct.SupplierId,
            SupplierName = updatedProduct.Supplier?.Name,
            CurrentStock = updatedProduct.GetCurrentStock(),
            IsLowStock = updatedProduct.IsLowStock(),
            CreatedAt = updatedProduct.CreatedAt
        };

        return Result.Success(productDto);
    }
}