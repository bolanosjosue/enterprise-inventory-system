using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Products.DTOs;
using MediatR;

namespace InventorySystem.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<Result<ProductDto>>
{
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public decimal Cost { get; init; }
    public int MinimumStock { get; init; }
    public int MaximumStock { get; init; }
    public Guid CategoryId { get; init; }
    public Guid? SupplierId { get; init; }
    public string? ImageUrl { get; init; }
}