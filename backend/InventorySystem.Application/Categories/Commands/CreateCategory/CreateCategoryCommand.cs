using InventorySystem.Application.Categories.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<Result<CategoryDto>>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}