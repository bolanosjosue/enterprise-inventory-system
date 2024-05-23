using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Products.DTOs;
using MediatR;

namespace InventorySystem.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<Result<PagedResult<ProductDto>>>
{
    public string? Search { get; init; }
    public Guid? CategoryId { get; init; }
    public string? Status { get; init; }
    public bool? LowStock { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}