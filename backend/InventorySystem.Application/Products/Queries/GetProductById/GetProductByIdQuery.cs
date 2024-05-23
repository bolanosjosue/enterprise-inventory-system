using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Products.DTOs;
using MediatR;

namespace InventorySystem.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;