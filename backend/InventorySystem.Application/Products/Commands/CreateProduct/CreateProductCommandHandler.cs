using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Products.DTOs;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IApplicationDbContext context,
        IRepository<Product> productRepository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Verificar si el SKU ya existe
        var skuExists = await _productRepository.ExistsAsync(
            p => p.Sku == request.Sku.ToUpperInvariant(),
            cancellationToken);

        if (skuExists)
        {
            return Result.Failure<ProductDto>($"A product with SKU '{request.Sku}' already exists");
        }

        // Verificar que la categoría existe
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return Result.Failure<ProductDto>("Category not found");
        }

        // Verificar que el proveedor existe (si se especificó)
        if (request.SupplierId.HasValue)
        {
            var supplierExists = await _context.Suppliers
                .AnyAsync(s => s.Id == request.SupplierId.Value, cancellationToken);
            if (!supplierExists)
            {
                return Result.Failure<ProductDto>("Supplier not found");
            }
        }

        // Crear el producto
        var product = Product.Create(
            request.Sku,
            request.Name,
            request.Price,
            request.Cost,
            request.MinimumStock,
            request.MaximumStock,
            request.CategoryId,
            request.SupplierId,
            request.Description,
            request.ImageUrl
        );

        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Obtener el producto con sus relaciones para el DTO
        var createdProduct = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstAsync(p => p.Id == product.Id, cancellationToken);

        var productDto = new ProductDto
        {
            Id = createdProduct.Id,
            Sku = createdProduct.Sku,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price,
            Cost = createdProduct.Cost,
            MinimumStock = createdProduct.MinimumStock,
            MaximumStock = createdProduct.MaximumStock,
            ImageUrl = createdProduct.ImageUrl,
            Status = createdProduct.Status.ToString(),
            CategoryId = createdProduct.CategoryId,
            CategoryName = createdProduct.Category.Name,
            SupplierId = createdProduct.SupplierId,
            SupplierName = createdProduct.Supplier?.Name,
            CurrentStock = createdProduct.GetCurrentStock(),
            IsLowStock = createdProduct.IsLowStock(),
            CreatedAt = createdProduct.CreatedAt
        };

        return Result.Success(productDto);
    }
}