using InventorySystem.Application.Categories.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryDto>>;